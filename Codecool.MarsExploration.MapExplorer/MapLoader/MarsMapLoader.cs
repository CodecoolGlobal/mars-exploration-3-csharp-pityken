using Codecool.MarsExploration.MapGenerator.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MapLoader;

public class MarsMapLoader : IMapLoader
{
    public Map Load(string mapFile)
    {
        string[] mapLines = ReadTextLinesFromFile(mapFile);
        string[,] mapMatrix = CreateMatrixRepresentation(mapLines);
        Map newMap = new(mapMatrix, true);
        return newMap;
    }

    private static string[,] CreateMatrixRepresentation(string[] strings)
    {
        int rowLength = strings.Length;
        int colLength = strings[0].Length;
        string[,] matrix = new string[rowLength, colLength];

        for (int i = 0; i < strings.Length; i++)
        {
            for (int j = 0; j < strings[i].Length; j++)
            {
                matrix[i, j] = strings[i][j].ToString();
            }
        }

        return matrix;
    }

    private static string[] ReadTextLinesFromFile(string path)
    {
        try
        {
            string[] text = File.ReadAllLines(path);
            return text;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
