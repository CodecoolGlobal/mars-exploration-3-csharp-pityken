using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

public class TimeoutAnalyzer : IAnalyzer
{
    public ExplorationOutcome Analyze(SimulationContext simulationContext)
    {
        if (simulationContext.CurrentStepNumber >= simulationContext.MaxSteps)
        {
            return ExplorationOutcome.Timeout;
        }
        return ExplorationOutcome.None;
    }
}
