using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;

public interface IFoundResourcesGenerator
{
    IEnumerable<FoundResource> Generate(SimulationContext simulationContext, int simulationId);
}
