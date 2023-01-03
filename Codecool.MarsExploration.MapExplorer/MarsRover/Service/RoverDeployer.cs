using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service;

public class RoverDeployer : IRoverDeployer
{
    private readonly Map _map;
    private readonly IMovementRoutine _exploringRoutine;
    private readonly IMovementRoutine _returningRoutine;
    private readonly IGatheringRoutine _gatheringRoutine;
    private readonly int _id;
    private readonly int _sight;
    private readonly Coordinate _deployPoint;
    private static readonly Random _random = new();

    public RoverDeployer(IMovementRoutine exploringRoutine, IMovementRoutine returningRoutine, int id, int sight, Coordinate shipLocation, Map map, IGatheringRoutine gatheringRoutine)
    {
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
        _deployPoint = shipLocation;
        _id = id;
        _sight = sight;
        _map = map;
        _gatheringRoutine = gatheringRoutine;
    }

    public Rover Deploy()
    {
        Coordinate[] adjacentCoordinates = _deployPoint.GetAdjacentCoordinates(_map.Dimension).ToArray();
        Coordinate? deployPosition = GetRandomEmptyAdjacentCoordinate(adjacentCoordinates);

        //foreach (var item in adjacentCoordinates)
        //    Console.WriteLine(item);
        //Console.WriteLine("Random empty: " + deployPosition);

        if (deployPosition is null)
            throw new Exception("Rover cannot be placed");

        return new Rover(_exploringRoutine, _returningRoutine, _gatheringRoutine, _id, deployPosition, _sight);
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

}
