using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.SimulationSteps;

public interface ISimulationStep
{
    ExplorationOutcome Step();
}