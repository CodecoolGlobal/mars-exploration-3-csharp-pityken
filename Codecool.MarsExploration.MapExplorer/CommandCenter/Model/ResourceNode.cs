using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.CommandCenter.Model;

public record ResourceNode
{
    
    public string Type { get; init; }
    public Coordinate Coordinate { get; init; }
    public bool HasRoverAssinged { get; set; }
	public ResourceNode(string type, Coordinate coordinate, bool hasRoverAssinged)
	{
        Type = type;
        Coordinate = coordinate;
        HasRoverAssinged = hasRoverAssinged;
    }
};

