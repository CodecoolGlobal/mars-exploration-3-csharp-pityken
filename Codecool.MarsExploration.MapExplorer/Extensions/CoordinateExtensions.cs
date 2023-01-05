using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Extensions;

public static class CoordinateExtensions
{
    public static IEnumerable<Coordinate> GetAdjacentCoordinatesAlt(this Coordinate coordinate, int mapDimension, int reach = 1)
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

    public static IEnumerable<Coordinate> GetAdjacentCoordinates(this Coordinate coordinate, int mapDimension, int reach = 1)
    {
        for (int x = Math.Max(coordinate.X - reach, 0); x <= Math.Min(coordinate.X + reach, mapDimension - 1); x++)
        {
            for (int y = Math.Max(coordinate.Y - reach, 0); y <= Math.Min(coordinate.Y + reach, mapDimension - 1); y++)
            {
                if ((x, y) == (coordinate.X, coordinate.Y)) continue;

                yield return new Coordinate(x, y);
            }
        }
    }
}
