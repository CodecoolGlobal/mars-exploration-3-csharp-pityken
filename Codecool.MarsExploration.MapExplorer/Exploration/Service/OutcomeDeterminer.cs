using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service
{
    public class OutcomeDeterminer : IOutcomeDeterminer
    {
        private readonly IAnalyzer _successAnalyzer;
        private readonly IAnalyzer _timeoutAnalyzer;

        public OutcomeDeterminer()
        {
            _successAnalyzer = new SuccessAnalyzer();
            _timeoutAnalyzer = new TimeoutAnalyzer();
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
