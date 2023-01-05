using Codecool.MarsExploration.MapExplorer.CommandCenter.Services;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapExplorer.Extensions;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Model;

public class CommandCenter
{
    public string Id { get; }
    public Coordinate Position { get; init; }
    public int Radius { get; init; }
    public List<Coordinate> AdjacentCoordinates { get; init; }
    public int BuildProgress { get; set; }
    public int AssemblyProgress { get; set; }
    public List<ResourceNode> ResourceNodes { get; init; }
    public Dictionary<string, int> Resources { get; set; }
    public bool ExploringRoverNeeded { get; init; }
    public CommandCenterStatus CommandCenterStatus { get; set; }
    private readonly ICommandCenterAction _roverBuilderAction;

    public CommandCenter(int id, Coordinate position, int radius, Dictionary<string, int> resources, bool exploringRoverNeeded, ICommandCenterAction roverBuilderAction)
    {
        Id = $"base-{id}";
        Position = position;
        Radius = radius;
        AdjacentCoordinates = position.GetAdjacentCoordinates(radius).ToList();
        ResourceNodes = null; //todo
        Resources = resources;
        ExploringRoverNeeded = exploringRoverNeeded;
        CommandCenterStatus = CommandCenterStatus.UnderConstruction;
        _roverBuilderAction = roverBuilderAction;
        BuildProgress = 0;
        AssemblyProgress = 0;
    }

    public void AddToResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
        {
            Resources.Add(resource.Key, resource.Value);
        }
    }


}
