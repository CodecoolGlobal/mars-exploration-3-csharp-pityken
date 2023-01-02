using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Model;

public record Rover
{
    public string Id { get; }
    public Coordinate CurrentPosition { get; private set; }
    public int Sight { get; }
    public Dictionary<string, HashSet<Coordinate>> ExploredObjects { get; set; }
    public List<Coordinate> PositionHistory { get; }
    private readonly IMovementRoutine _exploringRoutine;
    private readonly IMovementRoutine _returningRoutine;

    public Rover(IMovementRoutine exploringRoutine, IMovementRoutine returningRoutine, int id, Coordinate deployPosition, int sight)
    {
        Id = $"rover-{id}";
        Sight = sight;
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
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
}
