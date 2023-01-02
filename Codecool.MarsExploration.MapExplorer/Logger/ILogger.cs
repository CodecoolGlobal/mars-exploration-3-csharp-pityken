using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Logger;

public interface ILogger
{
    void Log(string message);
    void ActionLog(string actionType, string name, int stepNumber);
    void PositionLog(Coordinate coordinate, string name, int stepNumber);
    void OutcomeLog(ExplorationOutcome outcome, string name, int stepNumber);


}
