using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.BuildingRoutine;
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
    private readonly IBuildingRoutine _buildingRoutine;
    private int _id;
    private readonly int _sight;
    private readonly int _maxExplorationStepCount;
    private readonly Coordinate _deployPoint;
    private readonly int _maxRoverInventorySize;
    private static readonly Random _random = new();

    public RoverDeployer(IMovementRoutine exploringRoutine, IMovementRoutine returningRoutine, int id, int sight, Coordinate shipLocation, Map map, IGatheringRoutine gatheringRoutine, IBuildingRoutine buildingRoutine, int maxRoverInventorySize, int maxExplorationStepCount)
    {
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
        _gatheringRoutine = gatheringRoutine;
        _buildingRoutine = buildingRoutine;
        _deployPoint = shipLocation;
        _id = id;
        _sight = sight;
        _map = map;
        _maxRoverInventorySize = maxRoverInventorySize;
        _maxExplorationStepCount = maxExplorationStepCount;
    }

    public Rover Deploy(Coordinate? location = null)
    {
        Coordinate[] adjacentCoordinates = location == null ? _deployPoint.GetAdjacentCoordinates(_map.Dimension).ToArray() : location.GetAdjacentCoordinates(_map.Dimension).ToArray();
        Coordinate? deployPosition = GetRandomEmptyAdjacentCoordinate(adjacentCoordinates);

        //foreach (var item in adjacentCoordinates)
        //    Console.WriteLine(item);
        //Console.WriteLine("Random empty: " + deployPosition);

        if (deployPosition is null)
            throw new Exception("Rover cannot be placed");

        return new Rover(_exploringRoutine, _returningRoutine, _gatheringRoutine, _id++, deployPosition, _sight, _maxExplorationStepCount, _buildingRoutine, _maxRoverInventorySize);
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
