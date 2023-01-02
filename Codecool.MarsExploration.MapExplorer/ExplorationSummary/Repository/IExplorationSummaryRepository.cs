namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

public interface IExplorationSummaryRepository
{
    void AddExplorationSummary(Model.ExplorationSummary explorationSummary);
    int GetExplorationSummaryIdByTimeStamp(long timestamp);
}
