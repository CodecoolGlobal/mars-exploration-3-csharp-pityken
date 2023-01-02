using Codecool.MarsExploration.MapExplorer.Configuration.Model;
using Codecool.MarsExploration.MapExplorer.Configuration.Service;
using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapExplorerTest
{
    internal class ConfigurationValidatorTest
    {
        private readonly ConfigurationValidator _validator = new ConfigurationValidator(new MarsMapLoader());
        private readonly string _basePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Resources");

        //Testing test maps availability in order to avoid false breaking tests
        //Is it good practice?
        [TestCase("ConfigurationValidatorTest_emptyMap.map")]
        [TestCase("ConfigurationValidatorTest_impossibleLandingPosition.map")]
        [TestCase("ConfigurationValidatorTest_impossibleRoverPlacement.map")]
        public void TestFilePath(string fileName)
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_emptyMap.map");
            Assert.IsTrue(File.Exists(filePAth));
        }

        [Test]
        public void TestThatValidConfigurationRecordProduceTrueReturnValue()
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_emptyMap.map");
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = new string[] { "*", "%" };
            int maxSteps = 1000;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsTrue(actual);
        }

        [Test]
        public void TestThatIncorrectFileNameInConfigurationRecordProduceFalseReturnValue()
        {
            string filePAth = "";
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = new string[] { "*", "%" };
            int maxSteps = 1000;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }

        [TestCase(-1, 0)]
        [TestCase(-0, -1)]
        [TestCase(-1, -1)]
        [TestCase(3, 0)]
        [TestCase(0, 3)]
        [TestCase(3, 3)]
        public void TestThatInvalidLandingPositionProduceFalseReturnValue(int x, int y)
        {
            string filePAth = Path.Join(_basePath,"ConfigurationValidatorTest_emptyMap.map");
            Coordinate landingSpot = new Coordinate(x, y);
            string[] resourcesToFind = new string[] { "*", "%" };
            int maxSteps = 1000;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }

        [Test]
        public void TestThatImpossibleLandingPositionProduceFalseReturnValue()
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_impossibleLandingPosition.map");
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = new string[] { "*", "%" };
            int maxSteps = 1000;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }

        [Test]
        public void TestThatImpossibleRoverPlacementProduceFalseReturnValue()
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_impossibleRoverPlacement.map");
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = new string[] { "*", "%" };
            int maxSteps = 1000;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }

        [Test]
        public void TestThatMissingResourcesArrayProduceFalseReturnValue()
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_emptyMap.map");
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = Array.Empty<string>();
            int maxSteps = 1000;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }

        [Test]
        public void TestThatMaxStepsValueSetToZeroProduceFalseReturnValue()
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_emptyMap.map");
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = new string[] {"*", "%"};
            int maxSteps = 0;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }

        [Test]
        public void TestThatMaxStepsValueSetToLessThanZeroProduceFalseReturnValue()
        {
            string filePAth = Path.Join(_basePath, "ConfigurationValidatorTest_emptyMap.map");
            Coordinate landingSpot = new Coordinate(1, 1);
            string[] resourcesToFind = new string[] { "*", "%" };
            int maxSteps = -1;
            ConfigurationRecord configurationRecord = new ConfigurationRecord(filePAth, landingSpot, resourcesToFind, maxSteps);

            bool actual = _validator.Validate(configurationRecord);

            Assert.IsFalse(actual);
        }
    }
}
