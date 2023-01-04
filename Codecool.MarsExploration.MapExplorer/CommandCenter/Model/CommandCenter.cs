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

        int Minerals = Resources["mineral"];
        int Water = Resources["water"];

        

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

    private Dictionary<string, int> GetResourcesInRadius(List<ResourceNode> ResourceNodes)
    {
        //var objects = MarsRover.ExploredObjects;
        Dictionary<string, int> resourcesInRadius = new();
        foreach (var coord in AdjacentCoordinates) 
        {
            foreach(var res in ResourceNodes)
            {
                if(coord == res.Coordinate)
                {
                    if (resourcesInRadius[res.Type] > 0)
                    {
                        resourcesInRadius[res.Type] += 1;
                    }
                    else
    {
                        resourcesInRadius.Add(res.Type, 1);
                    }
                }
            }
        }
        return resourcesInRadius;
    }
}
