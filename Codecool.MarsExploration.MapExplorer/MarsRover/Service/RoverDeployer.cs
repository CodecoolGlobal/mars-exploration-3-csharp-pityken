using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service;

public class RoverDeployer : IRoverDeployer
{
    private readonly Map _map;
    private readonly IMovementRoutine _exploringRoutine;
    private readonly IMovementRoutine _returningRoutine;
    private readonly int _id;
    private readonly int _sight;
    private readonly Coordinate _shipLocation;
    private static readonly Random _random = new();

    public RoverDeployer(IMovementRoutine exploringRoutine, IMovementRoutine returningRoutine, int id, int sight, Coordinate shipLocation, Map map)
    {
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
        _shipLocation = shipLocation;
        _id = id;
        _sight = sight;
        _map = map;
    }

    public Rover Deploy()
    {
        Coordinate[] adjacentCoordinates = GetAdjacentCoordinates(_shipLocation, _map.Dimension).ToArray();
        Coordinate? deployPosition = GetRandomEmptyAdjacentCoordinate(adjacentCoordinates);

        //foreach (var item in adjacentCoordinates)
        //    Console.WriteLine(item);
        //Console.WriteLine("Random empty: " + deployPosition);

        if (deployPosition is null)
            throw new Exception("Rover cannot be placed");

        return new Rover(_exploringRoutine, _returningRoutine, _id, deployPosition, _sight);
    }

    private Coordinate? GetRandomEmptyAdjacentCoordinate(Coordinate[] adjacentCoordinates)
    {
        Coordinate[] emptyCoordinates = adjacentCoordinates
                                .Where(c => _map.Representation[c.X, c.Y] == " ")
                                .ToArray();

        //foreach (var item in emptyCoordinates)
        //    Console.WriteLine("empty: " + item);

        if (emptyCoordinates.Length == 0)
            return null;
        
        Coordinate randomEmptyCoordinate = emptyCoordinates[_random.Next(emptyCoordinates.Length)];

        return randomEmptyCoordinate;
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
