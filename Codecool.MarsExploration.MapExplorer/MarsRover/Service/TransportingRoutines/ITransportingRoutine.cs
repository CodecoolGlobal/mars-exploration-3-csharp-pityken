using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.TransportingRoutines;

public interface ITransportingRoutine
{
    bool MoveToCoordinate(Coordinate coordinate);
}
