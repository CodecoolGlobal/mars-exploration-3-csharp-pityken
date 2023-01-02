using Codecool.MarsExploration.MapExplorer.MapLoader;
using Codecool.MarsExploration.MapExplorer.MarsRover.Model;
using Codecool.MarsExploration.MapExplorer.MarsRover.Service;
using Codecool.MarsExploration.MapGenerator.Calculators.Model;
using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorerTest;

public class RoverDeployerTest
{
    private static readonly Random _random= new Random();

    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string mapFile = $@"{WorkDir}\Resources\exploration-0.map";

    private readonly static IMapLoader mapLoader = new MarsMapLoader();
    private static readonly Map _map = mapLoader.Load(mapFile);


    private static readonly object[] LandingSpotCoordinates = new object[]
    {
        new Coordinate(0, 0),
        new Coordinate(0, 2),
        new Coordinate(2, 0),
        new Coordinate(2, 2),
        new Coordinate(1, 2),
    };

    private static readonly object[] BiggerMapLandingSpotCoordinates = new object[]
    {
        new Coordinate(0, 0),
        new Coordinate(10, 9),
        new Coordinate(15, 5),
        new Coordinate(15, 6),
        new Coordinate(15, 15),
        new Coordinate(22, 3),
        new Coordinate(30, 30),
        new Coordinate(1, 13),
        new Coordinate(3, 17),
        new Coordinate(23, 5),
    };

    [Test]
    public void IfNoEmptyCordinatesFoundNearLandingSpot_ThrowsException()
    {
        Map map = new Map(new string[,] {
            { " ", "#", "#", "#" },
            { "#", "*", " ", "#" },
            { "#", "#", "#", "*" },
            { "#", "#", "#", " " },
        }, true);

        Coordinate landingSpot = new Coordinate(1, 2);
        IRoverDeployer roverDeployer = new RoverDeployer(null, null, 1, 2, landingSpot, map);

        Assert.That(() => roverDeployer.Deploy(), Throws.Exception
            .With.Property("Message").EqualTo($"Rover cannot be placed"));
    }


    [TestCaseSource(nameof(LandingSpotCoordinates))]
    public void IfAllAdjacentCoordinbatesAreEmpty_DeploysRoverOnRandomEmptyCoordinateOnMap(Coordinate landingCoordinate)
    {
        Map map = new Map(new string[,] {
            { " ", " ", " ", " " },
            { " ", " ", " ", " " },
            { " ", " ", " ", " " },
            { " ", " ", " ", " " },
        }, true);

        IRoverDeployer roverDeployer = new RoverDeployer(null, null, 1, 2, landingCoordinate, map);

        Rover deployedRover = roverDeployer.Deploy();
        Assert.Multiple(() =>
        {
            Assert.That(deployedRover.CurrentPosition.X, Is.InRange(0, map.Dimension));
            Assert.That(deployedRover.CurrentPosition.Y, Is.InRange(0, map.Dimension));
        });
    }


    [TestCaseSource(nameof(BiggerMapLandingSpotCoordinates))]
    public void IfOnlySomeEmptyCoordinateArePresent_FindsEmptyCoordinate(Coordinate landingCoordinate)
    {
        IRoverDeployer roverDeployer = new RoverDeployer(null, null, 1, 2, landingCoordinate, _map);

        Rover deployedRover = roverDeployer.Deploy();
        Console.WriteLine(deployedRover.CurrentPosition);

        Assert.That(_map.Representation[deployedRover.CurrentPosition.X, deployedRover.CurrentPosition.Y], Is.EqualTo(" "));
    }
}
