using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;
using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.SimulationSteps
{
    public class ColonizationSimulationStep : ISimulationStep
    {
        private readonly SimulationContext _simulationContext;
        private readonly IOutcomeDeterminer _outcomeDeterminer;
        private readonly IBuildableDeterminer _commandCenterBuildableDeterminer;
        private readonly IAssemblyRoutine _assemblyRoutine;
        private readonly IEnumerable<ILogger> _loggers;
        private readonly ICommandCenterDeployer commandCenterDeployer;
        private readonly IColonisationSummaryExporter _colonisationSummaryExporter;

        public ColonizationSimulationStep(SimulationContext simulationContext, IOutcomeDeterminer outcomeDeterminer, IEnumerable<ILogger> loggers, IBuildableDeterminer commandCenterBuildableDeterminer, IAssemblyRoutine asseblyRoutine, IColonisationSummaryExporter colonisationSummaryExporter)
        {
            _simulationContext = simulationContext;
            _outcomeDeterminer = outcomeDeterminer;
            _commandCenterBuildableDeterminer = commandCenterBuildableDeterminer;
            _loggers = loggers;
            _assemblyRoutine = asseblyRoutine;
            commandCenterDeployer = new CommandCenterDeployer(_simulationContext.CommandCenterRadius, _assemblyRoutine, (Dictionary<string, string>)_simulationContext.ResourcesToScan, _simulationContext.Map.Dimension);
            _colonisationSummaryExporter = colonisationSummaryExporter;
        }
        public ExplorationOutcome Step()
        {
            ActWithRovers();
            ActWithCommandCenters();
            _colonisationSummaryExporter.Export(_simulationContext);
            _simulationContext.CurrentStepNumber++;
            return GetExplorationOutcome();
        }

        private void ActWithRovers()
        {
            _simulationContext.Rovers.ForEach(r =>
            {
                if (!CommandCenterAssignedToRover(r))
                {
                    if (_commandCenterBuildableDeterminer.Determine(_simulationContext, r.Id))
                    {
                        var commandcenter = commandCenterDeployer.Deploy(r);
                        _simulationContext.CommandCenters.Add(commandcenter);
                        r.MoveBack();
                        ActionLog("deployment", r.Id, commandcenter.Id, null, null, commandcenter.Position);
                    }
                    else
                    {
                        r.Move(_simulationContext.Map.Dimension);
                        Scan(r);
                        PositionLog(r.CurrentPosition, r.Id);
                    }
                }
                else if (CommandCenterAssignedToRover(r) && ResourceNodeAssignedToRover(r))
                {
                    if (!BuildCommandCenterIfNeeded(r))
                    {
                        Scan(r);
                        var gatherstate = r.GatherResource(_simulationContext.Map.Dimension);

                        ActionLog(gatherstate.ToString(), r.Id, null, null, null, r.CurrentPosition);
                    }

                }
                else
                {
                    throw new Exception($"There is no ResourceNode assigned to the mining rover: {r.Id}");
                }
            });
        }

        private void ActWithCommandCenters()
        {
            _simulationContext.CommandCenters.ForEach(c =>
            {
                if (c.CommandCenterStatus != CommandCenter.Model.CommandCenterStatus.UnderConstruction)
                {
                    var roverStatus = c.UpdateStatus(_simulationContext.ResourcesNeededForRover);
                    if (roverStatus != null)
                    {
                        _simulationContext.Rovers.Add(roverStatus);
                        Dictionary<string, int> usedresources = new Dictionary<string, int>();
                        usedresources.Add("mineral", _simulationContext.ResourcesNeededForRover);
                        _colonisationSummaryExporter.ExportConstructionEvent(roverStatus.Id, c.Id, usedresources);
                        ActionLog("construction_complete", c.Id, roverStatus.Id);
                    }
                    if (c.CommandCenterStatus == CommandCenter.Model.CommandCenterStatus.RoverProduction)
                    {
                        ActionLog("construction", c.Id, null, (c.AssemblyProgress/10).ToString(), (10).ToString());
                    }
                }
            });
        }

        private ExplorationOutcome GetExplorationOutcome()
        {
            _simulationContext.ExplorationOutcome = _outcomeDeterminer.Determine(_simulationContext);
            if (_simulationContext.ExplorationOutcome != ExplorationOutcome.None)
            {
                OutComeLog();
            }

            return _simulationContext.ExplorationOutcome;
        }

        private bool CommandCenterAssignedToRover(Rover rover)
        {
            return rover.AssignedCommandCenter != null;
        }

        private bool ResourceNodeAssignedToRover(Rover rover)
        {
            return rover.ResourceNode != null;
        }

        private bool CommandCenterBesidesTheRover(Rover rover)
        {
            return rover.AssignedCommandCenter != null && rover.AssignedCommandCenter.Position.GetAdjacentCoordinates(_simulationContext.Map.Dimension).Contains(rover.CurrentPosition);
        }

        private bool BuildCommandCenterIfNeeded(Rover rover)
        {
            if (CommandCenterBesidesTheRover(rover)
                && CommandCenterHasNotBuiltYet(rover.AssignedCommandCenter)
                && HasAllResourcesToBuild(rover.AssignedCommandCenter))
            {
                rover.BuildCommandCenter();
                ActionLog("construction", rover.Id, rover.AssignedCommandCenter.Id, (rover.AssignedCommandCenter.BuildProgress/10).ToString(), "10");
                if (rover.AssignedCommandCenter.BuildProgress == 100)
                {
                    Dictionary<string, int> usedResources = new Dictionary<string, int>();
                    usedResources.Add("mineral", _simulationContext.ResourcesNeededForCommandCenter);
                    _colonisationSummaryExporter.ExportConstructionEvent(rover.AssignedCommandCenter.Id, rover.Id, usedResources);
                }
                return true;
            }

            return false;
        }

        private bool HasAllResourcesToBuild(CommandCenter.Model.CommandCenter commandCenter)
        {
            return commandCenter.IsConstructable(_simulationContext.ResourcesNeededForCommandCenter);
        }

        private bool CommandCenterHasNotBuiltYet(CommandCenter.Model.CommandCenter? commandCenter)
        {
            return commandCenter != null && commandCenter.CommandCenterStatus == CommandCenterStatus.UnderConstruction;
        }

        private void ActionLog(string actionType, string name, string? target = null, string? currentProgress = null, string? maxProgress = null, Coordinate? position = null)
        {
            foreach (var log in _loggers)
            {
                log.ActionLog(_simulationContext.CurrentStepNumber, actionType, name, target, currentProgress, maxProgress, position);
            }
        }

        private void OutComeLog()
        {
            foreach (var log in _loggers)
            {
                log.OutcomeLog(_simulationContext.CurrentStepNumber, _simulationContext.ExplorationOutcome);
            }
        }

        private void PositionLog(Coordinate position, string name)
        {
            foreach (var log in _loggers)
            {
                log.PositionLog(_simulationContext.CurrentStepNumber, position, name);
            }
        }

        private void Scan(Rover rover)
        {
            HashSet<Coordinate> coordinatesInSightDistance = GetAllCoordinatesInCurrentSightDistance(rover);

            //foreach (var item in coordinatesInSightDistance)
            //    Console.WriteLine(item);
            //Console.WriteLine(coordinatesInSightDistance.Count);

            ScanAllCoordinatesInSightDistance(coordinatesInSightDistance, rover);
            return;
        }

        private HashSet<Coordinate> GetAllCoordinatesInCurrentSightDistance(Rover rover)
        {
            HashSet<Coordinate> coordinatesInSightDistance = new HashSet<Coordinate>();

            HashSet<Coordinate> coordinatesInFirstReach = rover.CurrentPosition.GetAdjacentCoordinates(_simulationContext.Map.Dimension, rover.Sight).ToHashSet(); //GetAdjacentCoordinates(rover.CurrentPosition, _simulationContext.Map.Dimension).ToHashSet();
            coordinatesInSightDistance = coordinatesInSightDistance.Concat(coordinatesInFirstReach).ToHashSet();

            //coordinatesInSightDistance.Remove(rover.CurrentPosition);

            return coordinatesInSightDistance;
        }

        private void ScanAllCoordinatesInSightDistance(HashSet<Coordinate> coordinatesInSightDistance, Rover rover)
        {
            foreach (Coordinate coordinate in coordinatesInSightDistance)
            {
                string symbolOnCoordinate = _simulationContext.Map.Representation[coordinate.X, coordinate.Y];

                if (symbolOnCoordinate != " ")
                {
                    if (rover.ExploredObjects.ContainsKey(symbolOnCoordinate))
                        rover.ExploredObjects[symbolOnCoordinate].Add(coordinate);
                    else
                        rover.ExploredObjects.Add(symbolOnCoordinate, new HashSet<Coordinate> { coordinate });
                }
            }

            //Console.WriteLine("Current rover position: " + _simulationContext.Rover.CurrentPosition);
            //foreach (var kvp in _simulationContext.Rover.ExploredResources)
            //{
            //    Console.WriteLine(kvp.Key);
            //    foreach (var c in kvp.Value)
            //        Console.WriteLine(c);
            //}
        }
    }
}
