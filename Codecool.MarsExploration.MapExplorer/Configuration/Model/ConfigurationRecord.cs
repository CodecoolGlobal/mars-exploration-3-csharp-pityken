using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Model;

public record ConfigurationRecord(string MapPath, Coordinate LandingSpot, IDictionary<string, string> ResourcesToScan, int MaxSteps);

