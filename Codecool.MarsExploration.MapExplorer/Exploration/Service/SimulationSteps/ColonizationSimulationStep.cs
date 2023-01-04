using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
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
        private readonly IEnumerable<ILogger> _loggers;

        public ColonizationSimulationStep(SimulationContext simulationContext, IOutcomeDeterminer outcomeDeterminer, IEnumerable<ILogger> loggers)
        {
            _simulationContext = simulationContext;
            _outcomeDeterminer = outcomeDeterminer;
            _loggers = loggers;
        }
        public ExplorationOutcome Step()
        {
            ActWithRovers();
            ActWithCommandCenters();
            return GetExplorationOutcome();
        }

        private void ActWithRovers()
        {
            _simulationContext.Rovers.ForEach(r =>
            {
                if (!CommandCenterAssignedToRover(r))
                {
                    r.Move(_simulationContext.Map.Dimension);

                }
                else if (CommandCenterAssignedToRover(r) && ResourceNodeAssignedToRover(r))
                {
                    r.GatherResource(_simulationContext.Map.Dimension);
                    
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
                }
            });
        }

        private ExplorationOutcome GetExplorationOutcome()
        {
            throw new NotImplementedException();
        }

        private bool CommandCenterAssignedToRover(Rover rover)
        {
            return rover.CommandCenter != null;
        }

        private bool ResourceNodeAssignedToRover(Rover rover)
        {
            return rover.ResourceNode != null;
        }

        private bool CommandCenterBesidesTheRover(Rover rover)
        {
            return rover.CommandCenter != null ? rover.CommandCenter.AdjacentCoordinates.Contains(rover.CurrentPosition) : false;
        }

        private void CheckCommandCenterBuildRequirements(Rover rover)
        {
            if (CommandCenterBesidesTheRover(rover) && CommandCenterHasNotBuiltYet(rover.CommandCenter) && HasAllResourcesToBuild(rover.CommandCenter))
            {

            }
        }

        private bool HasAllResourcesToBuild(CommandCenter.Model.CommandCenter? commandCenter)
        {
            return true; // commandCenter != null && commandCenter.Resources.ContainsKey();
        }

        private bool CommandCenterHasNotBuiltYet(CommandCenter.Model.CommandCenter? commandCenter)
        {
            return commandCenter == null || commandCenter.CommandCenterStatus == CommandCenterStatus.UnderConstruction;
        }
    }
}
