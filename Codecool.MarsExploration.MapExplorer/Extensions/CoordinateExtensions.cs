using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Extensions;

public static class CoordinateExtensions
{
    public static IEnumerable<Coordinate> GetAdjacentCoordinates(this Coordinate coordinate, int mapDimension, int reach = 1)
    {
        Coordinate[] adjacent = new[]
        {
            coordinate with { Y = coordinate.Y + reach },
            coordinate with { Y = coordinate.Y - reach },
            coordinate with { X = coordinate.X + reach },
            coordinate with { X = coordinate.X - reach },
            coordinate with { X = coordinate.X + reach, Y = coordinate.Y + reach },
            coordinate with { X = coordinate.X - reach, Y = coordinate.Y + reach },
            coordinate with { X = coordinate.X + reach, Y = coordinate.Y - reach },
            coordinate with { X = coordinate.X - reach, Y = coordinate.Y - reach },
        };

        return adjacent.Where(c => c.X >= 0 && c.Y >= 0 && c.X < mapDimension && c.Y < mapDimension);
    }
}
