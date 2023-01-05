using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter
{
    public class ColonisationSummaryExporter : IColonisationSummaryExporter
    {
        private readonly IColonisationSummaryGenerator _generator;
        private readonly IColonisationSummaryRepository _repository;

        public ColonisationSummaryExporter(IColonisationSummaryGenerator generator, IColonisationSummaryRepository repository)
        {
            _generator = generator;
            _repository = repository;

            _repository.ResetAll();
        }

        public void Export(SimulationContext simulationContext)
        {
            var rovers = _generator.GenerateRoverSummaries(simulationContext);
            var commandCenters = _generator.GenerateCommandCenterSummaries(simulationContext);

            _repository.ResetCounterTypes();

            foreach (var rover in rovers)
            {
                _repository.AddRoverSummary(rover);
            }

            foreach (var commandCenter in commandCenters)
            {
                _repository.AddCommandCenterSummary(commandCenter);
            }
        }

        public void ExportConstructionEvent(string constructedObjectId, string constructorObjectId, Dictionary<string, int> resources)
        {
            var construction = _generator.GenerateConstructionSummary(constructedObjectId, constructorObjectId, resources);
            _repository.AddConstructionSummary(construction);
        }
    }
}