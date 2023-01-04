using Codecool.MarsExploration.MapExplorer.MarsRover.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;

public interface IAssemblyRoutine
{
    Rover? Assemble(Model.CommandCenter commandCenter);
}
