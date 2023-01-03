using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Logger;

public class ConsoleLogger : ILogger
{
    public void ActionLog(int stepNumber, string actionType, string name, string? target = null, string? currentProgress = null, string? maxProgress = null)
    {
        if (target == null || currentProgress == null || maxProgress == null)
        {
            Console.WriteLine($"STEP {stepNumber}; EVENT {actionType}; UNIT {name};");
        } else
        {
            Console.WriteLine($"STEP {stepNumber}; EVENT {actionType}; UNIT {name}; TARGET {target}; PROGRESS {currentProgress} of {maxProgress};");
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"STEP N/A; EVENT generic; MESSAGE {message};");
    }

    public void Log(string message, int stepNumber)
    {
        Console.WriteLine($"STEP {stepNumber}; EVENT generic; MESSAGE {message};");
    }

    public void OutcomeLog(int stepNumber, ExplorationOutcome outcome)
    {
        Console.WriteLine($"STEP {stepNumber}; OUTCOME {outcome};");
    }

    public void PositionLog(int stepNumber, Coordinate coordinate, string name)
    {
        Console.WriteLine($"STEP {stepNumber}; EVENT position; UNIT {name}; POSITION [{coordinate.X},{coordinate.Y}];");
    }
}
