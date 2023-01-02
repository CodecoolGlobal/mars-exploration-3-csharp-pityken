namespace Codecool.MarsExploration.MapExplorer.Logger;

public class FileLogger : ILogger
{
    private readonly string _currentLogFile;

    public FileLogger(string logFilePath)
    {
        _currentLogFile = logFilePath;

        string path = Path.GetDirectoryName(_currentLogFile) ?? string.Empty;
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (Exception)
        {
            throw;
        }

        try
        {
            File.WriteAllText(_currentLogFile, "");
        }
        catch (Exception)
        {
            throw;
        }

    }

    public void Log(string message)
    {
        WriteLineToLog(message, _currentLogFile);
    }

    private static void WriteLineToLog(string text, string path)
    {
        try
        {
            File.AppendAllText(path, text + Environment.NewLine);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
