﻿using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.BuildingRoutine;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Model;

public record Rover
{
    public string Id { get; }
    public int Sight { get; }
    public int InventorySize => _maxInventorySize;
    public int MaxExplorationStepCount { get; }
    public Coordinate CurrentPosition { get; private set; }
    public CommandCenter.Model.CommandCenter? AssignedCommandCenter { get; private set; }
    public Dictionary<string, int> Inventory { get; set; }
    public Dictionary<string, int> TotalCollectedResources { get; set; }
    public Dictionary<string, HashSet<Coordinate>> ExploredObjects { get; set; }
    public List<Coordinate> PositionHistory { get; }
    public int CurrentExplorationStepNumber { get; private set; }
    public ResourceNode? ResourceNode { get; private set; }
    public int StuckCounter { get; private set; }
    //public int AssemblyProgress { get; private set;  }

    private readonly IMovementRoutine _exploringRoutine;
    private readonly IMovementRoutine _returningRoutine;
    private readonly IGatheringRoutine _gatheringRoutine;
    private readonly IBuildingRoutine _buildingRoutine;
    private readonly int _maxInventorySize;
    private readonly Random _random;

    public Rover(IMovementRoutine exploringRoutine,
        IMovementRoutine returningRoutine,
        IGatheringRoutine gatheringRoutine,
        int id,
        Coordinate deployPosition,
        int sight,
        int maxExplorationStepCount,
        IBuildingRoutine buildingRoutine,
        int maxInventorySize)
    {
        Id = $"rover-{id}";
        Sight = sight;
        CurrentExplorationStepNumber = 0;
        _maxInventorySize = maxInventorySize;
        MaxExplorationStepCount = maxExplorationStepCount;
        _exploringRoutine = exploringRoutine;
        _returningRoutine = returningRoutine;
        _gatheringRoutine = gatheringRoutine;
        _buildingRoutine = buildingRoutine;
        Inventory = new Dictionary<string, int>();
        TotalCollectedResources = new Dictionary<string, int>();
        ExploredObjects = new Dictionary<string, HashSet<Coordinate>>();
        PositionHistory = new List<Coordinate>();
        _random = new Random();
        StuckCounter = 0;

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
            CurrentExplorationStepNumber++;
            return true;
        }
        return false;
    }

    public GatheringState GatherResource(int mapDimension)
    {
        (Coordinate, GatheringState) newCoordinateAndGatheringStatus = _gatheringRoutine.GatherResource(ResourceNode, AssignedCommandCenter, this, mapDimension);
        Coordinate newCoordinate = newCoordinateAndGatheringStatus.Item1;
        GatheringState gatheringState = newCoordinateAndGatheringStatus.Item2;

        //bool hasMoved = CheckCoordinateEquality(CurrentPosition, newCoordinate);
        //if (!hasMoved)
        //    return false;

        SetPosition(GetEscapeCoordinateIfRoverStuck(newCoordinate, mapDimension));
        return gatheringState;
    }

    public void BuildCommandCenter()
    {
        if (AssignedCommandCenter is not null && AssignedCommandCenter.AdjacentCoordinates.Contains(CurrentPosition))
        {
            _buildingRoutine.Build(AssignedCommandCenter);
        }
    }

    public void MoveBack()
    {
        Coordinate returnCoordinate = PositionHistory[PositionHistory.Count - 2];
        CurrentPosition = returnCoordinate;
    }

    public void AddToInventory(ResourceNode resource)
    {
        if (!Inventory.ContainsKey(resource.Type))
            Inventory.Add(resource.Type, 1);
        else
            Inventory[resource.Type]++;
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

    private Coordinate GetEscapeCoordinateIfRoverStuck(Coordinate coordinate, int mapDimension)
    {
        int positionHistoryCount = PositionHistory.Count;
        int historyEntryCounter = 0;
        HashSet<Coordinate> differentCoordinates = new HashSet<Coordinate>();
        for (int i = positionHistoryCount - 1; i > positionHistoryCount - 11; i--)
        {
            if (i > 0)
            {
                differentCoordinates.Add(PositionHistory[i]);
                historyEntryCounter++;
            }
        }


        if (differentCoordinates.Count < 2 && historyEntryCounter >= 10)
        {
            IEnumerable<Coordinate> adjecentCoordinates = coordinate.GetAdjacentCoordinates(mapDimension, 2).ToHashSet();
            HashSet<Coordinate> objectCoordinates = new HashSet<Coordinate>();
            ExploredObjects
                    .ToList()
                    .ForEach(
                        x => x.Value
                    .ToList()
                    .ForEach(c => objectCoordinates.Add(c))
                  );
            Coordinate[] emptyCoordinates = adjecentCoordinates.Except(objectCoordinates).ToArray();
            StuckCounter++;

            return emptyCoordinates[_random.Next(0, emptyCoordinates.Length)];
        }
        else if (differentCoordinates.Count > 9)
        {
            StuckCounter = 0;
        }
        return coordinate;
    }
}
