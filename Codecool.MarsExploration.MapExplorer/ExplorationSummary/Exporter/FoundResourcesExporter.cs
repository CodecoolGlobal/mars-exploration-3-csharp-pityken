using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter;

public class FoundResourcesExporter : IFoundResourcesExporter
{
    private readonly IFoundResourcesGenerator _foundResourcesGenerator;
    private readonly IFoundResourcesRepository _foundResourcesRepository;

    public FoundResourcesExporter(IFoundResourcesGenerator foundResourcesGenerator, IFoundResourcesRepository foundResourcesRepository)
    {
        _foundResourcesGenerator = foundResourcesGenerator;
        _foundResourcesRepository = foundResourcesRepository;
    }

     public void Export(SimulationContext simulationContext, int explorationSummaryId)
    {
        IEnumerable<FoundResource> resources = _foundResourcesGenerator.Generate(simulationContext, explorationSummaryId);
        foreach (var resource in resources)
        {
            _foundResourcesRepository.AddFoundResource(resource);
        }
    }
}
