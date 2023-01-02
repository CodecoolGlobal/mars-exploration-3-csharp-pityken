using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;

public interface IMovementRoutine
{
    Coordinate Move(int mapDimension, Dictionary<string, HashSet<Coordinate>> ExploredObjects, IList<Coordinate> positionHistory);
}
