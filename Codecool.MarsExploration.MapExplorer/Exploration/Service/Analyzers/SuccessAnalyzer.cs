using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

public class SuccessAnalyzer : IAnalyzer
{
    public ExplorationOutcome Analyze(SimulationContext simulationContext)
    {
        if (CheckColonizableAmountsOfResources(simulationContext)
            || CheckColonizableDistanceOfResources(simulationContext)
            || CheckColonizableDistanceFromLanding(simulationContext))
        {
            return ExplorationOutcome.Colonizable;
        }
        return ExplorationOutcome.None;
    }

    private static bool CheckColonizableAmountsOfResources(SimulationContext simulationContext)
    {
        int colonizableAmountOfMineral = 4;
        int colonizableAmountOfWater = 3;

        bool colonizableAmountOfMineralFound = false;
        bool colonizableAmountOfWaterFound = false;
        foreach (var exploredResource in simulationContext.Rovers[0].ExploredObjects)
        {
            if (exploredResource.Key == simulationContext.ResourcesToScan["mineral"] && exploredResource.Value.Count >= colonizableAmountOfMineral)
                colonizableAmountOfMineralFound = true;

            if (exploredResource.Key == simulationContext.ResourcesToScan["water"] && exploredResource.Value.Count >= colonizableAmountOfWater)
                colonizableAmountOfWaterFound = true;
        }
        return colonizableAmountOfMineralFound && colonizableAmountOfWaterFound;
    }

    private static bool CheckColonizableDistanceOfResources(SimulationContext simulationContext)
    {
        int colonizableWithinDistance = 5;
        string colonizableResourceA = simulationContext.ResourcesToScan["water"];
        string colonizableResourceB = simulationContext.ResourcesToScan["mineral"];

        bool colonizableDistanceOfResourcesFound = false;
        foreach (var exploredResource in simulationContext.Rovers[0].ExploredObjects)
        {
            if (exploredResource.Key == colonizableResourceA && exploredResource.Value.Count >= 1)
            {
                exploredResource.Value.ToList().ForEach(coordinate =>
                {
                    var targetResourceCoordinates = ListObjectCoordinatesFromRover(colonizableResourceB, simulationContext);
                    var coordinatesInRange = GetCoordinatesInRange(coordinate, colonizableWithinDistance, targetResourceCoordinates, simulationContext.Map.Dimension);

                    foreach (var targetCoordinate in coordinatesInRange)
                    {
                        if (CheckLineOfSight(coordinate, targetCoordinate, ListAllObjectCoordinatesFromRover(simulationContext)))
                            colonizableDistanceOfResourcesFound = true;
                    }

                });
            }
        }
        return colonizableDistanceOfResourcesFound;
    }

    private static bool CheckColonizableDistanceFromLanding(SimulationContext simulationContext)
    {
        int colonizableWithinDistance = 10;
        string colonizableResource = simulationContext.ResourcesToScan["water"];
        int colonizableResourceAmount = 2;

        var landingCoordinate = simulationContext.SpaceShipLocation;
        var targetResourceCoordinates = ListObjectCoordinatesFromRover(colonizableResource, simulationContext);
        var coordinatesInRange = GetCoordinatesInRange(landingCoordinate, colonizableWithinDistance, targetResourceCoordinates, simulationContext.Map.Dimension);

        int colonizableResourcesFound = 0;
        foreach (var targetCoordinate in coordinatesInRange)
        {
            if (CheckLineOfSight(landingCoordinate, targetCoordinate, ListAllObjectCoordinatesFromRover(simulationContext)))
                colonizableResourcesFound++;
        }

        if (colonizableResourcesFound >= colonizableResourceAmount)
        {
            return true;
        }
        return false;
    }

    private static bool CheckLineOfSight(Coordinate coordinateA, Coordinate coordinateB, List<Coordinate> mapObjects)
    {
        var delta = new Coordinate(
            coordinateB.X - coordinateA.X,
            coordinateB.Y - coordinateA.Y);

        int distance = Math.Max(
            Math.Abs(delta.X),
            Math.Abs(delta.Y));

        var direction = new Coordinate(
            Convert.ToInt32((double)delta.X / distance),
            Convert.ToInt32((double)delta.Y / distance));

        var (x, y) = (coordinateA.X, coordinateA.Y);
        for (int i = 0; i < distance; i++)
        {
            x += direction.X;
            y += direction.Y;

            if (i == distance - 1 && coordinateB != new Coordinate(x, y))
            {
                return false;
            }

            if (i != distance - 1 && mapObjects.Contains(new Coordinate(x, y)))
            {
                return false;
            }
        }
        return true;
    }

    private static List<Coordinate> ListObjectCoordinatesFromRover(string targetResource, SimulationContext simulationContext)
    {
        HashSet<Coordinate>? returnValue = simulationContext.Rovers[0].ExploredObjects.Where(er => er.Key == targetResource).FirstOrDefault().Value;
        return returnValue != null ? returnValue.ToList() : new List<Coordinate>();
    }

    private static List<Coordinate> ListAllObjectCoordinatesFromRover(SimulationContext simulationContext)
    {
        return simulationContext.Rovers[0].ExploredObjects.Values.SelectMany(er => er).ToList();
    }

    private static IEnumerable<Coordinate> GetCoordinatesInRange(Coordinate baseCoordinate, int maxDistance, List<Coordinate> targetCoordinates, int mapSize)
    {
        var nearbyCoordinates = GetAdjacentCoordinates(baseCoordinate, maxDistance, mapSize);
        foreach (var nearbyCoordinate in nearbyCoordinates)
        {
            if (targetCoordinates.Contains(nearbyCoordinate))
            {
                yield return nearbyCoordinate;
            }
        }
    }

    private static IEnumerable<Coordinate> GetAdjacentCoordinates(Coordinate coordinate, int distance, int size)
    {
        for (int x = Math.Max(coordinate.X - distance, 0); x <= Math.Min(coordinate.X + distance, size - 1); x++)
        {
            for (int y = Math.Max(coordinate.Y - distance, 0); y <= Math.Min(coordinate.Y + distance, size - 1); y++)
            {
                if ((x, y) == (coordinate.X, coordinate.Y)) continue;

                yield return new Coordinate(x, y);
            }
        }
    }

}
