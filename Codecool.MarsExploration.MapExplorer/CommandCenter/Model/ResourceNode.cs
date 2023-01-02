using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Model;

public record ResourceNode(string Type, Coordinate Coordinate, bool HasRoverAssinged);

