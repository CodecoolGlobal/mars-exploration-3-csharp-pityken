using Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Services;

public class CommandCenterDeployer : ICommandCenterDeployer
{
    private int _id;
    private int _radius;
    private readonly IAssemblyRoutine _assemblyRoutine;

    public CommandCenterDeployer(int radius, IAssemblyRoutine assemblyRoutine)
    {
        _id = 0;
        _radius = radius;
        _assemblyRoutine = assemblyRoutine;
    }

    public Model.CommandCenter Deploy(Rover rover)
    {
        _id++;

        IEnumerable<KeyValuePair<string, HashSet<Coordinate>>> sortedExploredObjects = rover.ExploredObjects.Where(
            exploredResource => exploredResource.Key == "mineral" || exploredResource.Key == "water"
        );
        Dictionary<string, HashSet<Coordinate>> sortedExploredObjectsDictionary = sortedExploredObjects.ToDictionary(x => x.Key, x => x.Value);

       return new Model.CommandCenter(_id, rover, rover.CurrentPosition, _radius, sortedExploredObjectsDictionary, false, _assemblyRoutine);
    }
}
