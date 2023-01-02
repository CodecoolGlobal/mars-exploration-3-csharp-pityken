using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter
{
    public class ExplorationSummaryExporter : IExplorationSummaryExporter
    {
        private readonly IExplorationSummaryGenerator _generator;
        private readonly IExplorationSummaryRepository _repository;

        public ExplorationSummaryExporter(IExplorationSummaryGenerator generator, IExplorationSummaryRepository repository)
        {
            _generator = generator;
            _repository = repository;
        }

        public int Export(SimulationContext simulationContext)
        {
            Model.ExplorationSummary summary = _generator.Generate(simulationContext);
            _repository.AddExplorationSummary(summary);
            return _repository.GetExplorationSummaryIdByTimeStamp(summary.TimeStamp);
        }
    }
}
