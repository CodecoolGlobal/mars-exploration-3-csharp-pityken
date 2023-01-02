using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

public interface IFoundResourcesRepository
{
    void AddFoundResource(FoundResource foundResource);
}
