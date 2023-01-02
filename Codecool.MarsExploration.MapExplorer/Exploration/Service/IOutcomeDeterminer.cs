using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public interface IOutcomeDeterminer
{
    ExplorationOutcome Determine(SimulationContext simulationContext);
}
