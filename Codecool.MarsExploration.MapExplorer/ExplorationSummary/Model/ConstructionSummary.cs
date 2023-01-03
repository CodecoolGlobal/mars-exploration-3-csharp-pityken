namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

public record ConstructionSummary(string ConstructedObjectId, string ConstructorObjectId, Dictionary<string, int> UsedResources);
