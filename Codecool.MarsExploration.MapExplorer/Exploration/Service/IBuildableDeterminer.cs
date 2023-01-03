using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public interface IBuildableDeterminer
{
    bool Determine(SimulationContext simulationContext, string roverId);
}
