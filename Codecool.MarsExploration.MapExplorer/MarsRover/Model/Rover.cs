using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.BuildingRoutine;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using System.Resources;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Model;

public record Rover
{
    public string Id { get; }
    public int Sight { get; }
    public int InventorySize { get; }
    public int MaxExplorationStepCount { get; }
    public Coordinate CurrentPosition { get; private set; }
    public CommandCenter.Model.CommandCenter? AssignedCommandCenter { get; private set; }
    public Dictionary<string, int> Inventory { get; set; }
    public Dictionary<string, HashSet<Coordinate>> ExploredObjects { get; set; }
    public List<Coordinate> PositionHistory { get; }
    public int CurrentExplorationStepNumber { get; private set; }
    public ResourceNode? ResourceNode { get; private set; }
    //public int AssemblyStatus { get; private set;  }

    private readonly IMovementRoutine _exploringRoutine;
    private readonly IMovementRoutine _returningRoutine;
    private readonly IGatheringRoutine _gatheringRoutine;
    private readonly IBuildingRoutine _buildingRoutine;


    public Rover(IMovementRoutine exploringRoutine,
        IMovementRoutine returningRoutine,
        IGatheringRoutine gatheringRoutine,
        int id,
        Coordinate deployPosition,
        int sight,
        int maxExplorationStepCount,
        IBuildingRoutine buildingRoutine
        )
    {
        Id = $"rover-{id}";
        Sight = sight;
        //AssemblyStatus = 0;
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
        _gatheringRoutine = gatheringRoutine;
        Inventory = new();
        ExploredObjects = new Dictionary<string, HashSet<Coordinate>>();
        PositionHistory = new List<Coordinate>();
        MaxExplorationStepCount = maxExplorationStepCount;

        SetPosition(deployPosition);
        _buildingRoutine = buildingRoutine;
        CurrentExplorationStepNumber = 0;
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
            CurrentExplorationStepNumber++;
            return true;
        }
        return false;
    }

    public bool GatherResource(int mapDimension)
    {
        Coordinate newCoordinate = _gatheringRoutine.GatherResource(ResourceNode, AssignedCommandCenter, this, mapDimension);
        bool hasMoved = CheckCoordinateEquality(CurrentPosition, newCoordinate);

        if (!hasMoved)
            return false;

        CurrentPosition = newCoordinate;
        return true;
    }

    public void BuildCommandCenter()
    {
        if (AssignedCommandCenter is not null && AssignedCommandCenter.AdjacentCoordinates.Contains(CurrentPosition))
        {
            _buildingRoutine.Build(AssignedCommandCenter);
        }
    }

    //public bool CheckCommandCenterBuildibility(int resourcesNeededForCommandCenter)
    //{
    //    if (AssignedCommandCenter is null)
    //        return false;

    //    return AssignedCommandCenter.CommandCenterStatus == CommandCenterStatus.UnderConstruction && AssignedCommandCenter.Resources.Count >= resourcesNeededForCommandCenter;
    //}

    //public void Assemble()
    //{
    //    AssemblyStatus += 1;
    //}

    public void AddToInventory(ResourceNode resource)
    {
        Inventory.Add(resource.Type, 1);
    }

    public void RemoveFromInventory(ResourceNode resource)
    {
        Inventory.Remove(resource.Type, out int removedItem);
    }

    public void AssignCommandCenter(CommandCenter.Model.CommandCenter commandCenter)
    {
        AssignedCommandCenter = commandCenter;
    }

    public void AssignResourceNode(ResourceNode resourceToAssign)
    {
        ResourceNode = resourceToAssign;
    }

    private bool CheckCoordinateEquality(Coordinate coordinateOne, Coordinate coordinateTwo)
    {
        return coordinateOne.X == coordinateTwo.X && coordinateOne.Y == coordinateTwo.Y;
    }


}
