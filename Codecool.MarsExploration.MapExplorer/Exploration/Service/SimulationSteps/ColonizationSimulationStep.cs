using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;
using Codecool.MarsExploration.MapExplorer.Exploration.Model;
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
        private readonly IAssemblyRoutine _asseblyRoutine;
        private readonly IEnumerable<ILogger> _loggers;
        private readonly ICommandCenterDeployer commandCenterDeployer;

        public ColonizationSimulationStep(SimulationContext simulationContext, IOutcomeDeterminer outcomeDeterminer, IEnumerable<ILogger> loggers, IBuildableDeterminer commandCenterBuildableDeterminer, IAssemblyRoutine asseblyRoutine)
        {
            _simulationContext = simulationContext;
            _outcomeDeterminer = outcomeDeterminer;
            _commandCenterBuildableDeterminer = commandCenterBuildableDeterminer;
            _loggers = loggers;
            _asseblyRoutine = asseblyRoutine;
            commandCenterDeployer = new CommandCenterDeployer(_simulationContext.CommandCenterRadius, _asseblyRoutine);
        }
        public ExplorationOutcome Step()
        {
            ActWithRovers();
            ActWithCommandCenters();
            //exportToDb
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
                        commandcenter.AssignResourceAndCommandCenterToTheRover(r);
                        r.MoveBack();
                        ActionLog("deployment", r.Id, commandcenter.Id, null, null, commandcenter.Position);
                    }
                    else
                    {
                        r.Move(_simulationContext.Map.Dimension);
                        PositionLog(r.CurrentPosition, r.Id);
                    }
                }
                else if (CommandCenterAssignedToRover(r) && ResourceNodeAssignedToRover(r))
                {
                    if (!BuildCommandCenterIfNeeded(r))
                    {
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
            if (CheckRovesForTimeout())
            {
                _simulationContext.ExplorationOutcome = ExplorationOutcome.Timeout;
                OutComeLog();
            }
            else if (CheckForColonizableOutcome(_simulationContext.CommandCentersNeeded))
            {
                _simulationContext.ExplorationOutcome = ExplorationOutcome.Colonizable;
                OutComeLog();
            }

            _simulationContext.ExplorationOutcome = ExplorationOutcome.None;

            return _simulationContext.ExplorationOutcome;
        }

        private bool CheckRovesForTimeout()
        {
            return _simulationContext.Rovers.Any(r => r.CurrentExplorationStepNumber >= _simulationContext.MaxSteps);
        }

        private bool CheckForColonizableOutcome(int targetNumberOfCommandCenters)
        {
            return targetNumberOfCommandCenters <= _simulationContext.CommandCenters.Count; //Does not work, we need the working command center in order to determine it colonizable
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
            return rover.AssignedCommandCenter != null && rover.AssignedCommandCenter.AdjacentCoordinates.Contains(rover.CurrentPosition);
        }

        private bool BuildCommandCenterIfNeeded(Rover rover)
        {
            if (CommandCenterBesidesTheRover(rover)
                && CommandCenterHasNotBuiltYet(rover.AssignedCommandCenter)
                && HasAllResourcesToBuild(rover.AssignedCommandCenter))
            {
                rover.BuildCommandCenter();
                ActionLog("construction", rover.Id, rover.AssignedCommandCenter.Id, (rover.AssignedCommandCenter.BuildProgress/10).ToString(), "10");
                return true;
            }

            return false;
        }

        private bool HasAllResourcesToBuild(CommandCenter.Model.CommandCenter commandCenter)
        {
            return commandCenter.IsConstructable(_simulationContext.ResourcesNeededForCommandCenter, commandCenter.Resources["mineral"]);
        }

        private bool CommandCenterHasNotBuiltYet(CommandCenter.Model.CommandCenter? commandCenter)
        {
            return commandCenter != null && commandCenter.CommandCenterStatus == CommandCenterStatus.UnderConstruction;
        }

        private void ActionLog(string actionType, string name, string? target = null, string? currentProgress = null, string? maxProgress = null, Coordinate? position = null)
        {
            foreach (var log in _loggers)
            {
                log.ActionLog(_simulationContext.CurrentStepNumber, actionType, name, target, currentProgress, maxProgress);
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
    }
}
