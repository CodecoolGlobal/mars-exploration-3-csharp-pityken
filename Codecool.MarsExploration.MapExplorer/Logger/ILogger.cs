using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Logger;

public interface ILogger
{
    void Log(string message);
    void Log(string message, int stepNumber);
    void ActionLog(int stepNumber, string actionType, string name, string? target, string? currentProgress, string? maxProgress);
    void PositionLog(int stepNumber, Coordinate coordinate, string name);
    void OutcomeLog(int stepNumber, ExplorationOutcome outcome);


}
