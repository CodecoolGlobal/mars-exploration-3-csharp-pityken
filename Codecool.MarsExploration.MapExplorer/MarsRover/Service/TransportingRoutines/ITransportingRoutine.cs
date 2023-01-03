using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.TransportingRoutines;

public interface ITransportingRoutine
{
    Coordinate MoveToCoordinate(Coordinate coordinate);
}
