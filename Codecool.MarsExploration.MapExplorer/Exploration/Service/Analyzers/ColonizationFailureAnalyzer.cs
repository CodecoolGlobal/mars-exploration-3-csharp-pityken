using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
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
        else if (RoverStuckCounterError(simulationContext.Rovers))
        {
            return ExplorationOutcome.Error;
        }
        return ExplorationOutcome.None;
    }

    private static bool RoverStuckCounterError(List<Rover> rovers)
    {
        return rovers.Any(r => r.StuckCounter > 10);
    }

    private static bool MapCoverageComplete(SimulationContext simulationContext)
    {
        int allExploredObjectsCount = AllExploredObjectsCounter(simulationContext);
        int mapObjectCount = MapObjectCounter(simulationContext);

        return allExploredObjectsCount >= mapObjectCount;
    }

    private static int AllExploredObjectsCounter(SimulationContext simulationContext)
    {
        HashSet<Coordinate> AllExploredObjects = new();
        foreach (var rover in simulationContext.Rovers)
        {
            rover.ExploredObjects
                .ToList()
                .ForEach(
                    x => x.Value
                    .ToList()
                    .ForEach(c => AllExploredObjects.Add(c))
            );
        }

        return AllExploredObjects.Count();
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
