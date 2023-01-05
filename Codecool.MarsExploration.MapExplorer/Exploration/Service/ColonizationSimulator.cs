using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service.SimulationSteps;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service
{
    internal class ColonizationSimulator : IExplorationSimulator
    {
        private readonly ISimulationStep _simulationStep;
        private readonly SimulationContext _simulationContext;
        public ColonizationSimulator(ISimulationStep simulationStep, SimulationContext simulationContext)
        {
            _simulationStep = simulationStep;
            _simulationContext = simulationContext;
        }
        public void Run()
        {
            while (_simulationContext.ExplorationOutcome == ExplorationOutcome.None)
            {
                _simulationContext.ExplorationOutcome = _simulationStep.Step();
            
        }
    }
}
