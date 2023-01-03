using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service
{
    public class ColonizationOutcomeDeterminer : IOutcomeDeterminer
    {
        private readonly IAnalyzer _successAnalyzer;

        public ColonizationOutcomeDeterminer()
        {
            _successAnalyzer = new ColonizationSuccessAnalyzer();
        }
        public ExplorationOutcome Determine(SimulationContext simulationContext)
        {
            ExplorationOutcome SuccessAnalyzerResult = _successAnalyzer.Analyze(simulationContext);

            if (SuccessAnalyzerResult == ExplorationOutcome.Colonizable)
            {
                return SuccessAnalyzerResult;
            }

            return ExplorationOutcome.None;
        }
    }
}
