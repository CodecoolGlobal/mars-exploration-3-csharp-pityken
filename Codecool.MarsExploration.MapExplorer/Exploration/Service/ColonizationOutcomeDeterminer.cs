using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service
{
    public class ColonizationOutcomeDeterminer : IOutcomeDeterminer
    {
        private readonly IAnalyzer _successAnalyzer;
        private readonly IAnalyzer _timeoutAnalyzer;

        public ColonizationOutcomeDeterminer()
        {
            _successAnalyzer = new ColonizationSuccessAnalyzer();
            _timeoutAnalyzer = new ColonizationTimeoutAnalyzer();
        }
        public ExplorationOutcome Determine(SimulationContext simulationContext)
        {
            ExplorationOutcome SuccessAnalyzerResult = _successAnalyzer.Analyze(simulationContext);
            ExplorationOutcome TimeoutAnalyzerResult = _timeoutAnalyzer.Analyze(simulationContext);

            if (SuccessAnalyzerResult == ExplorationOutcome.Colonizable)
            {
                return SuccessAnalyzerResult;
            }

            if (TimeoutAnalyzerResult == ExplorationOutcome.Timeout)
            {
                return TimeoutAnalyzerResult;
            }

            return ExplorationOutcome.None;
        }
    }
}
