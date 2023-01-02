using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

public interface IAnalyzer
{
    ExplorationOutcome Analyze(SimulationContext simulationContext); 
}
