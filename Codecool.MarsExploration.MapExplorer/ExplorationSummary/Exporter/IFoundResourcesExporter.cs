using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter;

public interface IFoundResourcesExporter
{
    void Export(SimulationContext simulationContext, int explorationSummaryId);
}
