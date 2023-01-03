using AStar;
using AStar.Options;
using Codecool.MarsExploration.MapExplorer.Extensions;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System.Drawing;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.TransportingRoutines
{
    internal class TransportingRoutine : ITransportingRoutine
    {
        private Map _map;
        public TransportingRoutine(Map map)
        {
            _map = map;
        }
        public Coordinate MoveToCoordinate(Coordinate coordinate, Coordinate roverCoordinate)
        {
            var pathfinderOptions = new PathFinderOptions
            {
                PunishChangeDirection = true,
                UseDiagonals = true,
            };

            var tiles = ConvertMap(_map);

            var worldGrid = new WorldGrid(tiles);
            var pathfinder = new PathFinder(worldGrid, pathfinderOptions);

            var path = pathfinder.FindPath(new Point(roverCoordinate.X, roverCoordinate.Y), new Point(coordinate.X, coordinate.Y));
            if (path is null)
            {
                return roverCoordinate;
            }
            return new Coordinate(path.First().X, path.First().Y);
        }

        private short[,] ConvertMap(Map map)
        {
            var horizontal = map.Representation.GetLength(1);
            var vertical = map.Representation.GetLength(0);
            var temp = new short[vertical, horizontal];

            for (int i = 0; i < vertical; i++)
            {
                for (int j = 0; j < horizontal; j++)
                {
                    if (map.Representation[i, j] == "")
                    {
                        temp[i, j] = 1;
                    }
                    else
                    {
                        temp[i, j] = 0;
                    }
                }
            }
            return temp;
        }
    }
}