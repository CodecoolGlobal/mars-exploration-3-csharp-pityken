using Codecool.MarsExploration.MapExplorer.Exploration.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.Analyzers;

public class ColonizationSuccessAnalyzer : IAnalyzer
{
    public ExplorationOutcome Analyze(SimulationContext simulationContext)
    {
        if (CheckColonizableAmountsOfCommandCenters(simulationContext))
        {
            return ExplorationOutcome.Colonizable;
        }
        return ExplorationOutcome.None;
    }

    private static bool CheckColonizableAmountsOfCommandCenters(SimulationContext simulationContext)
    {
        int colonizableAmountOfCommandCenters = 3;

        int amountOfWorkingCommandCenters = 0;
        foreach (var commandCenter in simulationContext.CommandCenters)
        {
            if (commandCenter.CommandCenterStatus != CommandCenter.Model.CommandCenterStatus.UnderConstruction)
                amountOfWorkingCommandCenters++;
        }
        return amountOfWorkingCommandCenters >= colonizableAmountOfCommandCenters;
    }
}
