namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

public record ConstructionSummary(string ConstructedObjectId, string ConstructorObjectId, List<ConstructionMaterialsSummary> Resources);
