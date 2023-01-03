using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;

public interface IGatheringRoutine
{
    Coordinate GatherResource(
        ResourceNode resourceNode, 
        Coordinate commandCenterCoordinate, 
        Coordinate RoverPsition, 
        Dictionary<string, int> Inventory, 
        int inventorySize);
}
