namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

public record ConstructionMaterialsSummary(string ResourceType, int UsedAmount)
{
    public int? ConstructionSummaryId { get; set; }
}
