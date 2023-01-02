using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;

public class RandomExploringRoutine : IMovementRoutine
{
    private readonly Random _random = new();
    public Coordinate Move(int mapDimension, Dictionary<string, HashSet<Coordinate>> ExploredObjects, IList<Coordinate> positionHistory)
    {
        Coordinate currentCoordinate = positionHistory[positionHistory.Count - 1];

        IEnumerable<Coordinate> adjacentCoordinates = GetAdjacentCoordinates(currentCoordinate, mapDimension);

        List<Coordinate>? emptyAdjacentCoordinates = GetEmptyAdjacentCoordinates(adjacentCoordinates, ExploredObjects);
        if (emptyAdjacentCoordinates is null)
            return currentCoordinate;
        
        return GetRandomCoordinate(emptyAdjacentCoordinates);
    }

    private bool CheckCoordinate(Coordinate coordinate, Dictionary<string, HashSet<Coordinate>> ExploredObjects)
    {
        foreach (KeyValuePair<string, HashSet<Coordinate>> kvp in ExploredObjects)
        {
            if (kvp.Value.Contains(coordinate))
                return false;
        }

        return true;
    }

    private Coordinate GetRandomCoordinate(List<Coordinate> coordinates)
    {
        return coordinates[_random.Next(coordinates.Count)];
    }

    private List<Coordinate>? GetEmptyAdjacentCoordinates(IEnumerable<Coordinate> adjacentCoordinates, Dictionary<string, HashSet<Coordinate>> ExploredObjects)
    {
        List<Coordinate> emptyCoordinates = adjacentCoordinates
                                .Where(c => CheckCoordinate(c, ExploredObjects) == true)
                                .ToList();
        if (!emptyCoordinates.Any())
            return null;

        return emptyCoordinates;
    }

    private IEnumerable<Coordinate> GetAdjacentCoordinates(Coordinate coordinate, int mapDimension, int reach = 1)
    {
        Coordinate[] adjacent = new[]
        {
            coordinate with { Y = coordinate.Y + reach },
            coordinate with { Y = coordinate.Y - reach },
            coordinate with { X = coordinate.X + reach },
            coordinate with { X = coordinate.X - reach },
            coordinate with { X = coordinate.X + reach, Y = coordinate.Y + reach },
            coordinate with { X = coordinate.X - reach, Y = coordinate.Y + reach },
            coordinate with { X = coordinate.X + reach, Y = coordinate.Y - reach },
            coordinate with { X = coordinate.X - reach, Y = coordinate.Y - reach },
        };

        return adjacent.Where(c => c.X >= 0 && c.Y >= 0 && c.X < mapDimension && c.Y < mapDimension);
    }
}
