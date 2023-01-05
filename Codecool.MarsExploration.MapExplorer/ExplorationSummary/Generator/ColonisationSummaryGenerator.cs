using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using System;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;

public class ColonisationSummaryGenerator : IColonisationSummaryGenerator
{
    public IEnumerable<RoverSummary> GenerateRoverSummaries(SimulationContext simulationContext)
    {
        List<RoverSummary> summaries = new();
        foreach (var rover in simulationContext.Rovers)
        {
            var resourceSummary = GenerateResourceSummaries(rover.Id, rover.TotalCollectedResources).ToList();
            var summary = new RoverSummary(rover.Id, resourceSummary);
            summaries.Add(summary);
        }
        return summaries;
    }

    public IEnumerable<CommandCenterSummary> GenerateCommandCenterSummaries(SimulationContext simulationContext)
    {
        List<CommandCenterSummary> summaries = new();
        foreach (var commandCenter in simulationContext.CommandCenters)
        {
            var resourceSummary = GenerateResourceSummaries(commandCenter.Id, commandCenter.TotalCollectedResources).ToList();
            var summary = new CommandCenterSummary(commandCenter.Id, resourceSummary);
            summaries.Add(summary);
        }
        return summaries;
    }
    public IEnumerable<ResourceSummary> GenerateResourceSummaries(string objectId, Dictionary<string, int> resources)
    {
        foreach (var resource in resources.ToList())
        {
            yield return new ResourceSummary(objectId, resource.Key, resource.Value);
        }
    }

    public ConstructionSummary GenerateConstructionSummary(string constructedObjectId, string constructorObjectId, Dictionary<string, int> resources)
    {
        var resourceSummary = GenerateConstructionMaterialsSummary(resources).ToList();
        var summary = new ConstructionSummary(constructedObjectId, constructorObjectId, resourceSummary);
        return summary;
    }

    public IEnumerable<ConstructionMaterialsSummary> GenerateConstructionMaterialsSummary(Dictionary<string, int> resources)
    {
        foreach (var resource in resources.ToList())
        {
            yield return new ConstructionMaterialsSummary(resource.Key, resource.Value);
        }
    }

}
