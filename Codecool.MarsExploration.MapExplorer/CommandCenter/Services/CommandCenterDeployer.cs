using Codecool.MarsExploration.MapExplorer.MarsRover.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Services;

public class CommandCenterDeployer : ICommandCenterDeployer
{
    private int _id;
    private int _;

    public CommandCenterDeployer()
    {
        _id = 0;
    }

    public Model.CommandCenter Deploy(Rover rover)
    {
        _id++;
       // return new Model.CommandCenter(_id, rover.CurrentPosition, );
    }
}
