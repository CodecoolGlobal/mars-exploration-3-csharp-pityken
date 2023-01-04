using Codecool.MarsExploration.MapExplorer.CommandCenter.Model;

namespace Codecool.MarsExploration.MapExplorer.MarsRover.Service.BuildingRoutine
{
    public class BuildingRoutine : IBuildingRoutine
    {
        public void Build(CommandCenter.Model.CommandCenter AssignedCommandCenter)
        {
            if (AssignedCommandCenter.CommandCenterStatus == CommandCenterStatus.UnderConstruction)
            {
                if (AssignedCommandCenter.BuildProgress < 100)
                {
                    AssignedCommandCenter.BuildProgress = IncrementProgress(AssignedCommandCenter.BuildProgress);
                }
                else { AssignedCommandCenter.CommandCenterStatus = CommandCenterStatus.Idle; }
            }
        }

        public int IncrementProgress(int progress)
        {
            return progress += 10;
        }
    }
}
