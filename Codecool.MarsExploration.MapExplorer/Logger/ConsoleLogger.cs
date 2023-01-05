using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Logger;

public class ConsoleLogger : ILogger
{
    public void ActionLog(
        int stepNumber, 
        string actionType, 
        string name, 
        string? target = null, 
        string? currentProgress = null, 
        string? maxProgress = null, 
        Coordinate? position = null)
    {
        string logstring = $"STEP {stepNumber}; EVENT {actionType}; UNIT {name};";
        if (target != null)
        {
            logstring += $" TARGET {target};";
        }
        if (currentProgress != null && maxProgress != null)
        {
            logstring += $" PROGRESS {currentProgress} of {maxProgress};";
        }
        if (position != null)
        {
            logstring += $" POSITION [{position.X},{position.Y}];";
        }

        Console.WriteLine(logstring);
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
