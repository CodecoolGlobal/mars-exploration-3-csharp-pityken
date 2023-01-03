using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;

public interface IGatheringRoutine
{
    bool GatherResource(ResourceNode resourceNode, Coordinate commandCenterCoordinate, Dictionary<string, int> Inventory, int inventorySize);
}
