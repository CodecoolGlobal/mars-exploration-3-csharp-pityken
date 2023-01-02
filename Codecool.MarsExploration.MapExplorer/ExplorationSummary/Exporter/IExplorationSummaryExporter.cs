using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Exporter
{
    public interface IExplorationSummaryExporter
    {
        public int Export(SimulationContext simulationContext);
    }
}
