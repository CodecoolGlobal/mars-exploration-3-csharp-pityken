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
    private readonly IAssemblyRoutine _assemblyRoutine;

    public CommandCenter(
        int id, 
        Rover builderRover, 
        Coordinate position, 
        int radius, 
        Dictionary<string, HashSet<Coordinate>> discoveredResources, 
        List<ResourceNode> resourceNodes, 
        bool exploringRoverNeeded, 
        IAssemblyRoutine assemblyRoutine)
    {
        Id = $"base-{id}";
        Position = position;
        Radius = radius;
        Resources = new Dictionary<string, int>(); //Resources in Inventory
        ResourceNodes = GetResourcesInSight(discoveredResources);
        ExploringRoverNeeded = exploringRoverNeeded;
        BuildProgress = 0;
        _assemblyRoutine = assemblyRoutine;
        AssemblyProgress = 0;
        AdjacentCoordinates = position.GetAdjacentCoordinates(radius).ToList();
        CommandCenterStatus = CommandCenterStatus.UnderConstruction;
        AssignResourceAndCommandCenterToTheRover(builderRover);
    }

    private void AssignResourceAndCommandCenterToTheRover(Rover rover)
    {
        rover.AssignCommandCenter(this);
        AssignResourceNodeToRover(rover);
    }

    public void AddToResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
        {
            Resources.Add(resource.Key, resource.Value);
        }
    }
    
    public void AssignResourceNodeToRover(Rover rover) //rover has built => run
    {
        var mineralResource = ResourceNodes.Count(r => r.HasRoverAssinged == true) == 0 
            ? ResourceNodes.First(x => x.Type == "mineral") 
            : ResourceNodes.First(x => x.HasRoverAssinged == false);

        rover.AssignResourceNode(mineralResource);
        mineralResource.HasRoverAssinged = true;
    }

    public Rover? CcUpdateStatus(int roverCost, int ccCost)
    {
        var numberOfRoversNeeded = ResourceNodes.Count();

        int Minerals = Resources["mineral"];
        int Water = Resources["water"];

        

        if (ResourceNodes.Any(x => !x.HasRoverAssinged) && Minerals >= roverCost )
        {
            CommandCenterStatus = CommandCenterStatus.RoverProduction;
            return _assemblyRoutine.Assemble(this);
        }

        if (IsConstructable(ccCost, Minerals))
        {
            CommandCenterStatus = CommandCenterStatus.UnderConstruction;
            return null;
        }

        if (ExploringRoverNeeded && ResourceNodes.Any(x => x.HasRoverAssinged))
        {
            CommandCenterStatus = CommandCenterStatus.RoverProduction;
            return _assemblyRoutine.Assemble(this);
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
