using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

public class ColonizationTimeoutAnalyzer : IAnalyzer
{
    public ExplorationOutcome Analyze(SimulationContext simulationContext)
    {
        bool timeout = false;
        foreach (var rover in simulationContext.Rovers)
        {
            if (rover.CurrentExplorationStepNumber >= simulationContext.MaxSteps)
            {
                
                timeout = true;
                break;
            }
        }

        if (timeout)
        {
            return ExplorationOutcome.Timeout;
        }
        else
        {
            return ExplorationOutcome.None;
        }
    }
}
