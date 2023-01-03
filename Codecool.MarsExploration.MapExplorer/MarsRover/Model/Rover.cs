using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Model;

public record Rover
{
    public string Id { get; }

    public int Sight { get; }
    public int InventorySize { get; }

    public Coordinate CurrentPosition { get; private set; }
    public Coordinate? CommandCenterCoordinate { get; }

    public Dictionary<string, int> Inventory { get; set; }
    public Dictionary<string, HashSet<Coordinate>> ExploredObjects { get; set; }
    public List<Coordinate> PositionHistory { get; }

    public ResourceNode? ResourceNode { get; }

    private readonly IMovementRoutine _exploringRoutine;
    private readonly IMovementRoutine _returningRoutine;
    private readonly IGatheringRoutine _gatheringRoutine;

    public Rover(IMovementRoutine exploringRoutine, IMovementRoutine returningRoutine, IGatheringRoutine gatheringRoutine, int id, Coordinate deployPosition, int sight)
    {
        Id = $"rover-{id}";
        Sight = sight;
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
        _gatheringRoutine = gatheringRoutine;
        Inventory = new();
        ExploredObjects = new Dictionary<string, HashSet<Coordinate>>();
        PositionHistory = new List<Coordinate>();

        SetPosition(deployPosition);
    }

    private void SetPosition(Coordinate coordinate)
    {
        CurrentPosition = coordinate;
        PositionHistory.Add(coordinate);
    }

    public bool Move(int mapDimension, bool toTheSpaceShip = false)
    {
        Coordinate oldPosition = CurrentPosition;
        Coordinate newPosition = toTheSpaceShip ? _returningRoutine.Move(mapDimension, ExploredObjects, PositionHistory) : _exploringRoutine.Move(mapDimension, ExploredObjects, PositionHistory);
        if (oldPosition != newPosition)
        {
            SetPosition(newPosition);
            return true;
        }
        return false;
    }

    public bool GatherResource()
    {
        throw new NotImplementedException();
    }
}
