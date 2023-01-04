using Codecool.MarsExploration.MapExplorer.MarsRover.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Services;

public interface ICommandCenterDeployer
{
    Model.CommandCenter Deploy(Rover rover);
}
