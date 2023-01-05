using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.GatheringRoutines;

public interface IGatheringRoutine
{
    (Coordinate, GatheringState) GatherResource(
        ResourceNode resourceNode,
        CommandCenter.Model.CommandCenter commandCenter,
        Rover rover,
        int mapDimension
        );
}
