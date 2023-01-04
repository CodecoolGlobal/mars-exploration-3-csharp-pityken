using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

public class ColonizationFailureAnalyzer : IAnalyzer
{
    public ExplorationOutcome Analyze(SimulationContext simulationContext)
    {
        if (MapCoverageComplete(simulationContext))
        {
            return ExplorationOutcome.NotColonizable;
        }
        return ExplorationOutcome.None;
    }

    private static bool MapCoverageComplete(SimulationContext simulationContext)
    {
        int allExploredObjectsCount = AllExploredObjectsCounter(simulationContext);
        int mapObjectCount = MapObjectCounter(simulationContext);

        return allExploredObjectsCount >= mapObjectCount;
    }

    private static int AllExploredObjectsCounter(SimulationContext simulationContext)
    {
        Dictionary<string, HashSet<Coordinate>> AllExploredObjects = new();
        foreach (var rover in simulationContext.Rovers)
        {
            rover.ExploredObjects.ToList().ForEach(x => AllExploredObjects.Add(x.Key, x.Value));
        }

        int allExploredObjectsCount = 0;
        foreach (var exploredObject in AllExploredObjects)
        {
            allExploredObjectsCount += exploredObject.Value.Count;
        }
        return allExploredObjectsCount;
    }

    private static int MapObjectCounter(SimulationContext simulationContext)
    {
        int mapObjectCount = 0;
        foreach (var elementRepresentation in simulationContext.Map.Representation)
        {
            if (elementRepresentation != " ")
            {
                mapObjectCount++;
            }
        }
        return mapObjectCount;
    }
}
