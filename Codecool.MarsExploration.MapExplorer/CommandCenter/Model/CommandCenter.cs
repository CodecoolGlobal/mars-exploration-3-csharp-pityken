using Codecool.MarsExploration.MapExplorer.CommandCenter.Services;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.CommandCenter.Services.AssemblingRoutine;

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
    private Rover MarsRover;
    private IAssemblyRoutine AssemblyRoutine;

    public CommandCenter(
        int id, 
        Rover builderRover, 
        Coordinate position, 
        int radius, 
        Dictionary<string, HashSet<Coordinate>> discoveredResources, 
        Dictionary<string, int> resources, 
        List<ResourceNode> resourceNodes, 
        bool exploringRoverNeeded, 
        IAssemblyRoutine assemblyRoutine,
        ICommandCenterAction roverBuilderAction)
    {
        Id = $"base-{id}";
        MarsRover = builderRover;
        Position = position;
        Radius = radius;
        Resources = resources; //Resources in Inventory
        ResourceNodes = GetResourcesInSight(discoveredResources);
        ExploringRoverNeeded = exploringRoverNeeded;
        _roverBuilderAction = roverBuilderAction;
        BuildProgress = 0;
        AssemblyRoutine = assemblyRoutine;
        AssemblyProgress = 0;
        AdjacentCoordinates = position.GetAdjacentCoordinates(radius).ToList();
        CommandCenterStatus = CommandCenterStatus.UnderConstruction;
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


    private List<ResourceNode> GetResourcesInSight(Dictionary<string, HashSet<Coordinate>> discoveredResources)
    {
        List<ResourceNode> resources = new List<ResourceNode>();
        foreach(var discResource in discoveredResources)
        {
            foreach(Coordinate coord in AdjacentCoordinates)
            {
                if(discResource.Value.Any(x => x.X == coord.X && x.Y == coord.Y))
                {
                    resources.Add(new ResourceNode(discResource.Key, coord, false));
                }
            }
        }
        return resources;
    }
}
