using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Logger;

public class FileLogger : ILogger
{
    private readonly string _currentLogFile;

    public FileLogger(string logFilePath)
    {
        _currentLogFile = logFilePath;

        string path = Path.GetDirectoryName(_currentLogFile) ?? string.Empty;
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (Exception)
        {
            throw;
        }

        try
        {
            File.WriteAllText(_currentLogFile, "");
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static void WriteLineToLog(string text, string path)
    {
        try
        {
            File.AppendAllText(path, text + Environment.NewLine);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void ActionLog(int stepNumber, string actionType, string name, string? target = null, string? currentProgress = null, string? maxProgress = null, Coordinate? position = null)
    {
        string logstring = $"STEP {stepNumber}; EVENT {actionType}; UNIT {name};";
        if (target != null)
        {
            logstring += $" TARGET {target};";
        }
        if (currentProgress != null && maxProgress != null)
        {
            logstring += $" PROGRESS[{currentProgress} OF {maxProgress};";
        }
        if(position != null)
        {
            logstring += $" POSITION [{position.X},{position.Y}];";
        }

        WriteLineToLog(logstring, _currentLogFile);
    }

    public void Log(string message)
    {
        WriteLineToLog($"STEP N/A; EVENT generic; MESSAGE {message};", _currentLogFile);
    }

    public void Log(string message, int stepNumber)
    {
        WriteLineToLog($"STEP {stepNumber}; EVENT generic; MESSAGE {message};", _currentLogFile);
    }

    public void OutcomeLog(int stepNumber, ExplorationOutcome outcome)
    {
        WriteLineToLog($"STEP {stepNumber}; OUTCOME {outcome};", _currentLogFile);
    }

    public void PositionLog(int stepNumber, Coordinate coordinate, string name)
    {
        WriteLineToLog($"STEP {stepNumber}; EVENT position; UNIT {name}; POSITION [{coordinate.X},{coordinate.Y}];", _currentLogFile);
    }

}
