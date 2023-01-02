using Codecool.MarsExploration.MapExplorer.Configuration.Model;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorer.Configuration.Service
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        private readonly IMapLoader _mapLoader;

        public ConfigurationValidator(IMapLoader mapLoader)
        {
            _mapLoader = mapLoader;
        }

        public bool Validate(ConfigurationRecord configurationRecord)
        {
            if (IsMapFilePathValid(configurationRecord.MapPath))
            {
                Map map = _mapLoader.Load(configurationRecord.MapPath);

                return 
                    IsCoordinateEmpty(map, configurationRecord.LandingSpot) 
                    && IsFreeSpotNearTheSpaceShip(map, configurationRecord.LandingSpot)
                    && IsResourcesSpecified(configurationRecord.ResourcesToScan)
                    && IsTimeoutValid(configurationRecord.MaxSteps);
            }
            return false;
            throw new NotImplementedException();
        }

        private bool IsMapFilePathValid(string filePath)
        {
            return File.Exists(filePath);
        }

        private bool IsCoordinateEmpty(Map map, Coordinate landingSpot)
        {
            int mapXLength = map.Representation.GetLength(0);
            int mapYLength = map.Representation.GetLength(1);
            bool xInRange = landingSpot.X > -1 && landingSpot.X < mapXLength;
            bool yInRange = landingSpot.Y > -1 && landingSpot.Y < mapYLength;

            return xInRange && yInRange ? map.Representation[landingSpot.X, landingSpot.Y] == " " : false;
        }


        private bool IsFreeSpotNearTheSpaceShip(Map map, Coordinate landingSpot)
        {
            foreach (Coordinate coordinate in GetAdjacentCoordinates(map, landingSpot))
            {
                if (IsCoordinateEmpty(map, coordinate))
                {
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<Coordinate> GetAdjacentCoordinates(Map map, Coordinate center)
        {
            int mapXLength = map.Representation.GetLength(0);
            int mapYLength = map.Representation.GetLength(1);

            for (int x = center.X - 1; x < mapXLength; x++)
            {
                for (int y = center.Y - 1; y < mapYLength; y++)
                {
                    if (x > -1 && y > -1 && center.X != x && center.Y != y)
                    {
                        yield return new Coordinate(x, y);
                    }
                }
            }
        }

        private bool IsResourcesSpecified(IDictionary<string, string> resourcesToScan)
        {
            return resourcesToScan.Count() > 0;
        }

        private bool IsTimeoutValid(int maxSteps)
        {
            return maxSteps > 0;
        }
    }
}
