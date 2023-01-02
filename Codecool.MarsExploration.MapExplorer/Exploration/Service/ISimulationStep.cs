using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service;

public interface ISimulationStep
{
    ExplorationOutcome Step();
}