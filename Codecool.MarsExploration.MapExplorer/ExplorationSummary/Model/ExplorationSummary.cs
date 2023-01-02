namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;

    public record ExplorationSummary(long TimeStamp, string LogfilePath, int NumberOfSteps, string FoundResources, string Outcome);

