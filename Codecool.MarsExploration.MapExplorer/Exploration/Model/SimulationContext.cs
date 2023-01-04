using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
//using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Model;

public record SimulationContext
{
    public int CurrentStepNumber { get; set; }
    public int MaxSteps { get; init; }
    public int CommandCentersNeeded { get; init; }
    public List<Rover> Rovers { get; init; }
    public List<CommandCenter.Model.CommandCenter> CommandCenters { get; init; }
    public Coordinate SpaceShipLocation { get; init; }
    public Map Map { get; init; }
    public IDictionary<string, string> ResourcesToScan { get; init; }
    public ExplorationOutcome ExplorationOutcome { get; set; }
    public string LogFilePath { get; init; }
    public int ResourcesNeededForCommandCenter { get; init; }
    public int ResourcesNeededForRover { get; init; }
    public int MaxRoverInventorySize { get; init; }

    public SimulationContext(int maxSteps, Rover startingRover, Coordinate spaceShipLocation, Map map, IDictionary<string, string> resourcesToScan, string logFilePath, int commandCentersNeeded, int resourcesNeededForCommandCenter, int resourcesNeededForRover, int maxRoverInventorySize)
    {
        CurrentStepNumber = 0;
        MaxSteps = maxSteps;
        Rovers = new();
        SpaceShipLocation = spaceShipLocation;
        Map = map;
        ResourcesToScan = resourcesToScan;
        ExplorationOutcome = ExplorationOutcome.None;
        LogFilePath = logFilePath;
        CommandCentersNeeded = commandCentersNeeded;

        Rovers.Add(startingRover);
        ResourcesNeededForCommandCenter = resourcesNeededForCommandCenter;
        ResourcesNeededForRover = resourcesNeededForRover;
        MaxRoverInventorySize = maxRoverInventorySize;
    }

}
