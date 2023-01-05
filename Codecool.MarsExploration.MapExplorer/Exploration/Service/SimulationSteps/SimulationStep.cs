using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;

namespace Codecool.MarsExploration.MapExplorer.Exploration.Service.SimulationSteps;

public class SimulationStep : ISimulationStep
{
    private readonly SimulationContext _simulationContext;
    private readonly IOutcomeDeterminer _outcomeDeterminer;
    private readonly IEnumerable<ILogger> _loggers;

    public SimulationStep(SimulationContext simulationContext, IOutcomeDeterminer outcomeDeterminer, IEnumerable<ILogger> loggers)
    {
        _simulationContext = simulationContext;
        _outcomeDeterminer = outcomeDeterminer;
        _loggers = loggers;
    }

    public ExplorationOutcome Step()
    {
        bool roverNeedsToReturn = _simulationContext.ExplorationOutcome != ExplorationOutcome.None;
        ExplorationOutcome currentExplorationOutcome = Move(roverNeedsToReturn)
            ? _simulationContext.ExplorationOutcome
            : ExplorationOutcome.Error;

        if (currentExplorationOutcome == ExplorationOutcome.None)
        {
            StepIncrement();
            Scan();
            currentExplorationOutcome = Analyze();
            Log(currentExplorationOutcome);
        }

        return currentExplorationOutcome;
    }

    private bool Move(bool roverNeedsToReturn)
    {
        return _simulationContext.Rovers[0].Move(_simulationContext.Map.Dimension, roverNeedsToReturn);
    }

    private void Scan()
    {
        HashSet<Coordinate> coordinatesInSightDistance = GetAllCoordinatesInCurrentSightDistance();

        //foreach (var item in coordinatesInSightDistance)
        //    Console.WriteLine(item);
        //Console.WriteLine(coordinatesInSightDistance.Count);

        ScanAllCoordinatesInSightDistance(coordinatesInSightDistance);
        return;
    }

    private ExplorationOutcome Analyze()
    {
        return _outcomeDeterminer.Determine(_simulationContext);
    }

    private void Log(ExplorationOutcome currentExplorationOutcome)
    {
        foreach (ILogger logger in _loggers)
        {
            logger.Log($"STEP {_simulationContext.CurrentStepNumber}; EVENT position; UNIT {_simulationContext.Rovers[0].Id}; POSITION [{_simulationContext.Rovers[0].CurrentPosition.X}, {_simulationContext.Rovers[0].CurrentPosition.Y}]");

            if (currentExplorationOutcome != ExplorationOutcome.None)
                logger.Log($"STEP {_simulationContext.CurrentStepNumber}; EVENT outcome; OUTCOME {Enum.GetName(currentExplorationOutcome).ToUpper()}");
        }
        return;
    }

    private void StepIncrement()
    {
        _simulationContext.CurrentStepNumber++;
    }

    private void ScanAllCoordinatesInSightDistance(HashSet<Coordinate> coordinatesInSightDistance)
    {
        foreach (Coordinate coordinate in coordinatesInSightDistance)
        {
            string symbolOnCoordinate = _simulationContext.Map.Representation[coordinate.X, coordinate.Y];

            if (symbolOnCoordinate != " ")
            {
                if (_simulationContext.Rovers[0].ExploredObjects.ContainsKey(symbolOnCoordinate))
                    _simulationContext.Rovers[0].ExploredObjects[symbolOnCoordinate].Add(coordinate);
                else
                    _simulationContext.Rovers[0].ExploredObjects.Add(symbolOnCoordinate, new HashSet<Coordinate> { coordinate });
            }
        }

        //Console.WriteLine("Current rover position: " + _simulationContext.Rover.CurrentPosition);
        //foreach (var kvp in _simulationContext.Rover.ExploredResources)
        //{
        //    Console.WriteLine(kvp.Key);
        //    foreach (var c in kvp.Value)
        //        Console.WriteLine(c);
        //}
    }

    private HashSet<Coordinate> GetAllCoordinatesInCurrentSightDistance()
    {
        HashSet<Coordinate> coordinatesInSightDistance = new HashSet<Coordinate>();

        HashSet<Coordinate> coordinatesInFirstReach = GetAdjacentCoordinates(_simulationContext.Rovers[0].CurrentPosition, _simulationContext.Map.Dimension).ToHashSet();
        coordinatesInSightDistance = coordinatesInSightDistance.Concat(coordinatesInFirstReach).ToHashSet();

        if (_simulationContext.Rovers[0].Sight > 1)
        {
            for (int i = 2; i <= _simulationContext.Rovers[0].Sight; i++)
            {
                foreach (Coordinate coord in coordinatesInSightDistance)
                {
                    HashSet<Coordinate> adjacentOfCurrent = GetAdjacentCoordinates(coord, _simulationContext.Map.Dimension).ToHashSet();
                    coordinatesInSightDistance = coordinatesInSightDistance.Concat(adjacentOfCurrent).ToHashSet();
                }
            }
        }
        coordinatesInSightDistance.Remove(_simulationContext.Rovers[0].CurrentPosition);

        return coordinatesInSightDistance;
    }

    private IEnumerable<Coordinate> GetAdjacentCoordinates(Coordinate coordinate, int mapDimension, int reach = 1)
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
