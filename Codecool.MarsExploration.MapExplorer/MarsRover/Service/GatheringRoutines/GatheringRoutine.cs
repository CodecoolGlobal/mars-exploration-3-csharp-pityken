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


    public (Coordinate, GatheringState) GatherResource(ResourceNode resourceNode, CommandCenter.Model.CommandCenter commandCenter, Rover rover, int  mapDimension)
    {

        bool hasCollectedResource = rover.Inventory.Any();
        IEnumerable<Coordinate> adjacentCoordinatesOfResource = resourceNode.Coordinate.GetAdjacentCoordinates(mapDimension);
        IEnumerable<Coordinate> adjacentCoordinatesOfCommandCenter = commandCenter.Position.GetAdjacentCoordinates(mapDimension);

        if (adjacentCoordinatesOfResource.Contains(rover.CurrentPosition) && !hasCollectedResource)
        {
            int inventoryCount = 0;
            while (inventoryCount <= rover.InventorySize)
            {
                foreach (KeyValuePair<string, int> inventoryElement in rover.Inventory)
                {
                    inventoryCount += inventoryElement.Value;
                }
                rover.AddToInventory(resourceNode);

                if (!rover.TotalCollectedResources.ContainsKey(resourceNode.Type))
                   rover.TotalCollectedResources.Add(resourceNode.Type, 1);
                else
                    rover.TotalCollectedResources[resourceNode.Type]++;
            }

            return (rover.CurrentPosition, GatheringState.extraction);
        }

        if (adjacentCoordinatesOfCommandCenter.Contains(rover.CurrentPosition) && hasCollectedResource)
        {
            commandCenter.AddToResources(rover.Inventory);

            while (rover.Inventory.Count > 0)
            {
                rover.RemoveFromInventory(resourceNode);
            }
           
            return (rover.CurrentPosition, GatheringState.unload);
        }

        if (!hasCollectedResource)
        {
            Coordinate newCoordinate = _transportingRoutine.MoveToCoordinate(GetEmptyAdjecentCoordinate(adjacentCoordinatesOfResource, rover), rover.CurrentPosition);
            return (newCoordinate, GatheringState.delivery);
        }
        else
        {
            Coordinate newCoordinate = _transportingRoutine.MoveToCoordinate(GetEmptyAdjecentCoordinate(adjacentCoordinatesOfCommandCenter, rover), rover.CurrentPosition);
            return (newCoordinate, GatheringState.delivery);
        }
    }

    private Coordinate GetEmptyAdjecentCoordinate(IEnumerable<Coordinate> coordinates, Rover rover)
    {
        
        Dictionary<string, HashSet<Coordinate>> exploredObjects = rover.ExploredObjects;
        List<Coordinate> coordinatesWithObjects = new List<Coordinate>();
        foreach (KeyValuePair<string, HashSet<Coordinate>> kvp in exploredObjects) 
        {
            coordinatesWithObjects.AddRange(kvp.Value.ToList());
        }
        return coordinates.Where(c => !coordinatesWithObjects.Contains(c)).First();
    }
}
