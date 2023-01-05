using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter;

public interface IColonisationSummaryExporter
{
    void Export(SimulationContext simulationContext);
    void ExportConstructionEvent(string constructedObjectId, string constructorObjectId, Dictionary<string, int> resources);
}
