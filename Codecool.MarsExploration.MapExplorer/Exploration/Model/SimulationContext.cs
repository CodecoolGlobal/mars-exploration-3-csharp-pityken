using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Model;

public record SimulationContext
{
    public int CurrentStepNumber { get; set; }
    public int MaxSteps { get; init; }
    public Rover Rover { get; init; }
    public Coordinate SpaceShipLocation { get; init; }
    public Map Map { get; init; }
    public IDictionary<string, string> ResourcesToScan { get; init; }
    public ExplorationOutcome ExplorationOutcome { get; set; }
    public string LogFilePath { get; init; }

    public SimulationContext(int maxSteps, Rover rover, Coordinate spaceShipLocation, Map map, IDictionary<string, string> resourcesToScan, string logFilePath)
    {
        CurrentStepNumber = 0;
        MaxSteps = maxSteps;    
        Rover = rover;
        SpaceShipLocation = spaceShipLocation;
        Map = map;
        ResourcesToScan = resourcesToScan;
        ExplorationOutcome = ExplorationOutcome.None;
        LogFilePath = logFilePath;
    }

}
