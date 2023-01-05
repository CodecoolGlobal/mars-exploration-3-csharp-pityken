using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;
using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        CommandCenter.Model.CommandCenter commandcenter = commandCenterDeployer.Deploy(r);
                        commandcenter.AssignResourceAndCommandCenterToTheRover(r);
                        r.MoveBack();
                        //log
                    }
                    else
                    {
                        r.Move(_simulationContext.Map.Dimension);
                        //log
                    }
                }
                else if (CommandCenterAssignedToRover(r) && ResourceNodeAssignedToRover(r))
                {
                    if (!BuildCommandCenterIfNeeded(r))
                    {
                        r.GatherResource(_simulationContext.Map.Dimension);
                        //log
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
                    c.UpdateStatus();
                    //log
                }
            });
        }

        private ExplorationOutcome GetExplorationOutcome()
        {
            if (CheckRovesForTimeout())
            {
                //log
                return ExplorationOutcome.Timeout;
            }
            else if (CheckForColonizableOutcome(_simulationContext.CommandCentersNeeded))
            {
                //log
                return ExplorationOutcome.Colonizable;
            }

            return ExplorationOutcome.None;
        }

        private bool CheckRovesForTimeout()
        {
            return _simulationContext.Rovers.Any(r => r.CurrentExplorationStepNumber >= _simulationContext.MaxSteps);
        }

        private bool CheckForColonizableOutcome(int targetNumberOfCommandCenters)
        {
            return targetNumberOfCommandCenters <= _simulationContext.CommandCenters.Count;
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
            return rover.AssignedCommandCenter != null ? rover.AssignedCommandCenter.AdjacentCoordinates.Contains(rover.CurrentPosition) : false;
        }

        private bool BuildCommandCenterIfNeeded(Rover rover)
        {
            if (CommandCenterBesidesTheRover(rover)
                && CommandCenterHasNotBuiltYet(rover.AssignedCommandCenter) 
                && HasAllResourcesToBuild(rover.AssignedCommandCenter))
            {
                rover.BuildCommandCenter();
                //log
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
    }
}
