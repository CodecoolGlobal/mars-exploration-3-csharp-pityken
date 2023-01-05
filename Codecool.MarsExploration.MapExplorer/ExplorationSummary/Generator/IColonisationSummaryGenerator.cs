using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using System.Resources;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;

public interface IColonisationSummaryGenerator
{
    IEnumerable<RoverSummary> GenerateRoverSummaries(SimulationContext simulationContext);
    IEnumerable<CommandCenterSummary> GenerateCommandCenterSummaries(SimulationContext simulationContext);
    IEnumerable<ResourceSummary> GenerateResourceSummaries(string objectId, Dictionary<string, int> resources);
    ConstructionSummary GenerateConstructionSummary(string constructedObjectId, string constructorObjectId, Dictionary<string, int> resources);
    IEnumerable<ConstructionMaterialsSummary> GenerateConstructionMaterialsSummary(Dictionary<string, int> resources);
}
