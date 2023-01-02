using Codecool.MarsExploration.MapExplorer.Exploration.Model;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Generator
{
    public class ExplorationSummaryGenerator : IExplorationSummaryGenerator
    {
        public Model.ExplorationSummary Generate(SimulationContext simulationContext)
        {
            long timeStamp = DateTime.Now.Ticks;
            string foundResources = FoundResourcesToString(simulationContext);
            string outcome = simulationContext.ExplorationOutcome.ToString();
            return new Model.ExplorationSummary(timeStamp, simulationContext.LogFilePath, simulationContext.CurrentStepNumber, foundResources, outcome);
        }

        private string FoundResourcesToString(SimulationContext simulationContext)
        {
            List<string> foundResources = new List<string>();

            foreach (KeyValuePair<string, HashSet<Coordinate>> kvp in simulationContext.Rover.ExploredObjects)
            {
                if(simulationContext.ResourcesToScan.Values.Contains(kvp.Key))
                foundResources.Add($"{kvp.Key}={kvp.Value.Count}");
            }

            return string.Join("; ", foundResources);
        }
    }
}
