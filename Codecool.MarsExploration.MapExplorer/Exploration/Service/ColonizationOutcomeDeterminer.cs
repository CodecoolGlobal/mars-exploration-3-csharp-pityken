using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service
{
    public class ColonizationOutcomeDeterminer : IOutcomeDeterminer
    {
        private readonly IAnalyzer _successAnalyzer;
        private readonly IAnalyzer _timeoutAnalyzer;
        private readonly IAnalyzer _failureAnalyzer;

        public ColonizationOutcomeDeterminer()
        {
            _successAnalyzer = new ColonizationSuccessAnalyzer();
            _timeoutAnalyzer = new ColonizationTimeoutAnalyzer();
            _failureAnalyzer= new ColonizationFailureAnalyzer();
        }
        public ExplorationOutcome Determine(SimulationContext simulationContext)
        {
            ExplorationOutcome SuccessAnalyzerResult = _successAnalyzer.Analyze(simulationContext);
            ExplorationOutcome TimeoutAnalyzerResult = _timeoutAnalyzer.Analyze(simulationContext);
            ExplorationOutcome FailureAnalyzerResult = _failureAnalyzer.Analyze(simulationContext);

            if (SuccessAnalyzerResult != ExplorationOutcome.None)
            {
                return SuccessAnalyzerResult;
            }

            if (FailureAnalyzerResult != ExplorationOutcome.None)
            {
                return FailureAnalyzerResult;
            }

            if (TimeoutAnalyzerResult != ExplorationOutcome.None)
            {
                return TimeoutAnalyzerResult;
            }

            return ExplorationOutcome.None;
        }
    }
}
