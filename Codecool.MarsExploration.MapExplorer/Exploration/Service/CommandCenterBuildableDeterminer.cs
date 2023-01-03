using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public class CommandCenterBuildableDeterminer : IBuildableDeterminer
{
    public bool Determine(SimulationContext simulationContext, string roverId)
    {
        Rover selectedRover = FindRover(simulationContext, roverId);
        List<Coordinate> selectedRoverVisibleCoordinates = selectedRover.CurrentPosition.GetAdjacentCoordinates(simulationContext.Map.Dimension, selectedRover.Sight).ToList();

        return EnoughResourcesInSight(simulationContext, selectedRoverVisibleCoordinates) && !CommandCentersRadiusOverlapWithSight(simulationContext, selectedRoverVisibleCoordinates);
    }

    private bool EnoughResourcesInSight(SimulationContext simulationContext, List<Coordinate> visibleCoordinates)
    {
        int colonizableAmountOfMineral = 4;
        int colonizableAmountOfWater = 3;

        int amountOfMineralFound = 0;
        int amountOfWaterFound = 0;
        foreach (var coordinate in visibleCoordinates)
        {
            if (simulationContext.Map.Representation[coordinate.X, coordinate.Y] == simulationContext.ResourcesToScan["mineral"])
                amountOfMineralFound++;

            if (simulationContext.Map.Representation[coordinate.X, coordinate.Y] == simulationContext.ResourcesToScan["water"])
                amountOfWaterFound++;
        }
        return amountOfMineralFound >= colonizableAmountOfMineral && amountOfWaterFound >= colonizableAmountOfWater;
    }

    private bool CommandCentersRadiusOverlapWithSight(SimulationContext simulationContext, List<Coordinate> visibleCoordinates)
    {
        bool overlap = false;
        foreach (var commandCenter in simulationContext.CommandCenters)
        {
            List<Coordinate> CoordinatesInRadius = commandCenter.Position.GetAdjacentCoordinates(simulationContext.Map.Dimension, commandCenter.Radius).ToList();
            foreach (var coordinate in CoordinatesInRadius)
            {
                if (visibleCoordinates.Contains(coordinate))
                {
                    overlap = true;
                    break;
                }
            }
        }
        return overlap;
    }


    private Rover FindRover(SimulationContext simulationContext, string roverId)
    {
        var roverFound = simulationContext.Rovers.Where(r => r.Id == roverId).FirstOrDefault();
        if (roverFound != null)
        {
            return roverFound;
        }
        else
        {
            throw new Exception();
        }

    }
}
