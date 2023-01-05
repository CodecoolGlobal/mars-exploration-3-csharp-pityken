using Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Services;

public class CommandCenterDeployer : ICommandCenterDeployer
{
    private int _id;
    private int _radius;
    private readonly int _mapDimension;
    private readonly IAssemblyRoutine _assemblyRoutine;
    private readonly Dictionary<string, string> _resourceTypes;

    public CommandCenterDeployer(int radius, IAssemblyRoutine assemblyRoutine, Dictionary<string, string> resourceTypes, int mapDimension)
    {
        _id = 0;
        _radius = radius;
        _assemblyRoutine = assemblyRoutine;
        _resourceTypes = resourceTypes;
        _mapDimension = mapDimension;
    }

    public Model.CommandCenter Deploy(Rover rover)
    {
        _id++;

        IEnumerable<KeyValuePair<string, HashSet<Coordinate>>> sortedExploredObjects = rover.ExploredObjects.Where(
            exploredResource => exploredResource.Key == _resourceTypes["mineral"] || exploredResource.Key == _resourceTypes["water"]
        );
        Dictionary<string, HashSet<Coordinate>> sortedExploredObjectsDictionary = sortedExploredObjects.ToDictionary(x => x.Key, x => x.Value);

       return new Model.CommandCenter(_id, rover, rover.CurrentPosition, _radius, _mapDimension, sortedExploredObjectsDictionary, true, _assemblyRoutine);
    }
}
