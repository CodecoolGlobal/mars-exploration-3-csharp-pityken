using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines
{
    public class BasicReturningRoutine : IMovementRoutine
    {
        public Coordinate Move(int mapDimension, Dictionary<string, HashSet<Coordinate>> ExploredObjects, IList<Coordinate> positionHistory)
        {
            return positionHistory.ToList()[0];
        }
    }
}
