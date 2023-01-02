using Codecool.MarsExploration.MapExplorer.MapLoader;
using System.IO;

namespace Codecool.MarsExploration.MapExplorerTest;

public class MarsMapLoaderTest
{
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    private IMapLoader _mapLoader;

    public MarsMapLoaderTest()
    {
        _mapLoader = new MarsMapLoader();
    }

    [Test]
    public void MapLoadingTest()
    {
        string[] maps =
        {
        $"{WorkDir}\\Resources\\exploration-0.map",
        $"{WorkDir}\\Resources\\exploration-1.map",
        $"{WorkDir}\\Resources\\exploration-2.map",
        };

        foreach (var map in maps)
        {
            var actual = _mapLoader.Load(map);
            var expected = File.ReadAllLines(map);
            Assert.That(actual.Representation.GetLength(0), Is.EqualTo(expected.Length));

            for (int i = 0; i < actual.Representation.GetLength(0); i++)
            {
                for (int j = 0; j < actual.Representation.GetLength(0) - 1; j++)
                {
                    Assert.That(actual.Representation[i, j], Is.EqualTo(expected[i][j].ToString()));
                }

            }
        }
    }

    public void MapLoadingExceptionTest()
    {
        string map = "";
        Assert.Throws<Exception>(() => _mapLoader.Load(map));
    }
}
