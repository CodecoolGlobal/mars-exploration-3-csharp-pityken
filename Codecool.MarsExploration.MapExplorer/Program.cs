using Codecool.MarsExploration.MapExplorer.Configuration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.Exploration.Service;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;
using Codecool.MarsExploration.MapExplorer.Logger;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service.MovementRoutines;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using Codecool.MarsExploration.MapExplorer.Configuration.Service;

namespace Codecool.MarsExploration.MapExplorer;

class Program
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        string mapFile = $@"{WorkDir}\Resources\exploration-0.map";
        string repositoryFile = $"{WorkDir}\\Resources\\exploration_summaries.db";
        Coordinate landingSpot = new Coordinate(6, 6);
        Dictionary<string, string> resourcesToScan = new() { { "water", "*" }, { "mineral", "%" } };
        int maxSteps = 1000;
        string logFilePath = $"{WorkDir}\\Logs\\{DateTime.Now:yyyyMMdd_HHmmss}.log";

        ConfigurationRecord configuration = new ConfigurationRecord(mapFile, landingSpot, resourcesToScan, maxSteps);

        MarsMapLoader marsMapLoader = new MarsMapLoader();
        IConfigurationValidator configurationValidator = new ConfigurationValidator(marsMapLoader);

        bool isConfigurationValid = configurationValidator.Validate(configuration);

        if (isConfigurationValid)
        {
            Map map = marsMapLoader.Load(mapFile);

            IMovementRoutine exploringRoutine = new RandomExploringRoutine();
            IMovementRoutine returningRoutine = new BasicReturningRoutine();

            int id = 1;
            int sight = 5;
            IRoverDeployer roverDeployer = new RoverDeployer(exploringRoutine, returningRoutine, id, sight, configuration.LandingSpot, map);
            Rover MarsRover = roverDeployer.Deploy();

            SimulationContext simulationContext = new SimulationContext(configuration.MaxSteps, MarsRover, configuration.LandingSpot, map, configuration.ResourcesToScan, logFilePath);

            IEnumerable<ILogger> loggers = new List<ILogger>()
            {
                new ConsoleLogger(),
                new FileLogger(logFilePath),
            };

            IOutcomeDeterminer outcomeDeterminer = new OutcomeDeterminer();
            ISimulationStep simulationStep = new SimulationStep(simulationContext, outcomeDeterminer, loggers);

            IExplorationSummaryGenerator explorationSimulationGenerator = new ExplorationSummaryGenerator();
            IExplorationSummaryRepository explorationSummaryRepository = new ExplorationSummaryRepository(repositoryFile);
            IExplorationSummaryExporter explorationSummaryExporter = new ExplorationSummaryExporter(explorationSimulationGenerator, explorationSummaryRepository);
            IFoundResourcesGenerator foundResourcesGenerator = new FoundResourcesGenerator();
            IFoundResourcesRepository foundResourcesRepository = new FoundResourcesRepository(repositoryFile);
            IFoundResourcesExporter foundResourcesExporter = new FoundResourcesExporter(foundResourcesGenerator, foundResourcesRepository);
            
            IExplorationSimulator explorationSimulator = new ExplorationSimulator(simulationContext, simulationStep, explorationSummaryExporter, foundResourcesExporter);

            explorationSimulator.Run();

        }
        else
        {
            Console.WriteLine("Invalid Configuration!");
            Environment.Exit(-1);
        }
    }
}