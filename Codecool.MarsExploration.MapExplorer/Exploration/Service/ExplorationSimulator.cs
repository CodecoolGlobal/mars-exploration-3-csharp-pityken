using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service.SimulationSteps;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service
{
    public class ExplorationSimulator : IExplorationSimulator
    {
        private readonly SimulationContext _simulationContext;
        private readonly ISimulationStep _simulationStep;
        private readonly IExplorationSummaryExporter _explorationSummaryExporter;
        private readonly IFoundResourcesExporter _foundResourcesExporter;

        public ExplorationSimulator(SimulationContext simulationContext, ISimulationStep simulationStep, IExplorationSummaryExporter explorationSummaryExporter, IFoundResourcesExporter foundResourcesExporter)
        {
            _simulationContext = simulationContext;
            _simulationStep = simulationStep;
            _explorationSummaryExporter = explorationSummaryExporter;
            _foundResourcesExporter = foundResourcesExporter;
        }

        public void Run()
        {
            while (RoverNeedsToMove())
            {
                _simulationContext.ExplorationOutcome = _simulationStep.Step();
            }

            int summaryId = _explorationSummaryExporter.Export(_simulationContext);
            _foundResourcesExporter.Export(_simulationContext, summaryId);
        }

        private bool RoverNeedsToMove()
        {
            return !RoverReturnedToTheSpaceship() && _simulationContext.ExplorationOutcome != ExplorationOutcome.Error;
        }

        private bool RoverReturnedToTheSpaceship()
        {
            return _simulationContext.ExplorationOutcome != ExplorationOutcome.None && RoverBesidesTheSpaceShip();
        }

        private bool RoverBesidesTheSpaceShip()
        {
            return
                Math.Abs(_simulationContext.Rovers[0].CurrentPosition.X - _simulationContext.SpaceShipLocation.X) <= 1
                && Math.Abs(_simulationContext.Rovers[0].CurrentPosition.Y - _simulationContext.SpaceShipLocation.Y) <= 1;
        }
    }
}
