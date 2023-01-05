using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

public interface IColonisationSummaryRepository
{
    void AddRoverSummary(RoverSummary roverSummary);
    void AddCommandCenterSummary(CommandCenterSummary commandCenterSummary);
    void AddResourceSummary(string target, ResourceSummary resourceSummary);
    void AddConstructionSummary(ConstructionSummary constructionSummary);
    void AddConstructionMaterialsSummary(string target, ConstructionMaterialsSummary constructionMaterialsSummary);
    void ResetCounterTypes();
    void ResetAll();
}
