using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;

public class AssemblyRoutine : IAssemblyRoutine
{
    private readonly IRoverDeployer _roverDeployer;
    public Rover? Assemble(Model.CommandCenter commandCenter)
    {
        if (commandCenter.CommandCenterStatus == CommandCenterStatus.RoverProduction)
        {
            if (commandCenter.AssemblyProgress < 100)
            {
                commandCenter.AssemblyProgress += 10;
                return null;
            }
            else
            {
                commandCenter.AssemblyProgress = 0;
                return _roverDeployer.Deploy(); // create and return a new rover object
            }
        }
        else
        {
            return null;
        }
    }

    public AssemblyRoutine(IRoverDeployer roverDeployer)
    {
        _roverDeployer = roverDeployer;
    }
}
