using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.TransportingRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;


namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;

public class GatheringRoutine : IGatheringRoutine
{
    private readonly ITransportingRoutine _transportingRoutine;

    public GatheringRoutine(ITransportingRoutine transportingRoutine)
    {
        _transportingRoutine = transportingRoutine;
    }


    public Coordinate GatherResource(ResourceNode resourceNode, CommandCenter.Model.CommandCenter commandCenter, Rover rover, int  mapDimension)
    {

        bool hasCollectedResource = rover.Inventory.Any();
        IEnumerable<Coordinate> adjacentCoordinatesOfResource = resourceNode.Coordinate.GetAdjacentCoordinates(mapDimension);
        IEnumerable<Coordinate> adjacentCoordinatesOfCommandCenter = commandCenter.Position.GetAdjacentCoordinates(mapDimension);

        if (adjacentCoordinatesOfResource.Contains(rover.CurrentPosition) && !hasCollectedResource)
        {
            while (rover.Inventory.Count < rover.InventorySize)
            {
                rover.Inventory.Add(resourceNode.Type, 1);
            }
            return rover.CurrentPosition;
        }

        if (adjacentCoordinatesOfCommandCenter.Contains(rover.CurrentPosition) && hasCollectedResource)
        {
            commandCenter.AddToResources(rover.Inventory);
            return rover.CurrentPosition;
        }

        if (!hasCollectedResource)
        {
            Coordinate newCoordinate = _transportingRoutine.MoveToCoordinate(resourceNode.Coordinate);
            return newCoordinate;
        }
        else
        {
            Coordinate newCoordinate = _transportingRoutine.MoveToCoordinate(commandCenter.Position);
            return newCoordinate;
        }
    }
}
