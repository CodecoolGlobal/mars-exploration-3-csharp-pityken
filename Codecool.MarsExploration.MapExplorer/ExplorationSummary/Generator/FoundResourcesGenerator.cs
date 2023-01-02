using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;

public class FoundResourcesGenerator : IFoundResourcesGenerator
{
    public IEnumerable<FoundResource> Generate(SimulationContext simulationContext, int simulationId)
    {
        foreach (var resource in simulationContext.ResourcesToScan)
        {
            var foundResource = GetFoundResources(simulationContext.Rover.ExploredObjects, resource.Value);
            foreach(var coordinate in foundResource)
            {
                yield return new FoundResource(simulationId, resource.Key, resource.Value, coordinate.X, coordinate.Y);
            }
        }


    }

    private List<Coordinate> GetFoundResources(Dictionary<string, HashSet<Coordinate>> foundObjects, string representation)
    {
        return foundObjects[representation].ToList();
    }
}
