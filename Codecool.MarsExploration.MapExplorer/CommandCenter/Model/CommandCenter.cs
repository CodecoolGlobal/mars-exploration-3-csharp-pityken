using Codecool.MarsExploration.MapExplorer.CommandCenter.Services;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;

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
    

    public CommandCenter(
        int id, 
        Rover builderRover, 
        Coordinate position, 
        int radius, 
        Dictionary<string, int> resources, 
        Dictionary<string, HashSet<Coordinate>> discoveredResources, 
        List<ResourceNode> resourceNodes, 
        bool exploringRoverNeeded, 
        ICommandCenterAction roverBuilderAction)
    {
        Id = $"base-{id}";
        Position = position;
        Radius = radius;
        AdjacentCoordinates = position.GetAdjacentCoordinates(radius).ToList();
        ResourceNodes = resourceNodes;
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

    public Rover? CcUpdateStatus(int roverCost, int ccCost)
    {
        var numberOfRoversNeeded = ResourceNodes.Count();
        int Minerals = 0;
        int Water = 0;

        if (ResourceNodes.Any(x => !x.HasRoverAssinged) && Minerals >= roverCost )
        {
            CommandCenterStatus = CommandCenterStatus.RoverProduction;
            //assemble rover
            return null;
        }

        if (IsConstructable(ccCost, Minerals))
        {
            CommandCenterStatus = CommandCenterStatus.UnderConstruction;
            //idonno what else
            return null;
        }

        if (ExploringRoverNeeded && ResourceNodes.Any(x => x.HasRoverAssinged))
        {
            CommandCenterStatus = CommandCenterStatus.RoverProduction;
            //assemble rover
            return null; 
        }

        CommandCenterStatus = CommandCenterStatus.Idle;
        return null;


    }

    public bool IsConstructable(int resourceNeeded, int totalResource)
    {
        if (CommandCenterStatus == CommandCenterStatus.UnderConstruction && resourceNeeded >= totalResource)
        { 
            return true;
        }
        return false;
    }

    private void GetResourcesInRadius()
    {
        
    }
}
