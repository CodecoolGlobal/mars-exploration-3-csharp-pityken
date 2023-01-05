﻿using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
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
            while (rover.Inventory.Count < rover.InventorySize)
            {
                rover.AddToInventory(resourceNode);

                if (!rover.TotalCollectedResources.ContainsKey(resourceNode.Type))
                   rover.TotalCollectedResources.Add(resourceNode.Type, 1);
                else
                    rover.TotalCollectedResources[resourceNode.Type]++;
            }

            return (rover.CurrentPosition, GatheringState.Extraction);
        }

        if (adjacentCoordinatesOfCommandCenter.Contains(rover.CurrentPosition) && hasCollectedResource)
        {
            commandCenter.AddToResources(rover.Inventory);

            while (rover.Inventory.Count > 0)
            {
                rover.RemoveFromInventory(resourceNode);
            }
           
            return (rover.CurrentPosition, GatheringState.Unload);
        }

        if (!hasCollectedResource)
        {
            Coordinate newCoordinate = _transportingRoutine.MoveToCoordinate(resourceNode.Coordinate, rover.CurrentPosition);
            return (newCoordinate, GatheringState.Delivery);
        }
        else
        {
            Coordinate newCoordinate = _transportingRoutine.MoveToCoordinate(commandCenter.Position, rover.CurrentPosition);
            return (newCoordinate, GatheringState.Delivery);
        }
    }
}
