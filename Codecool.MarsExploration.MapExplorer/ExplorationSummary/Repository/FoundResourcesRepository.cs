using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using Microsoft.Data.Sqlite;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

public class FoundResourcesRepository : IFoundResourcesRepository
{
    private readonly string _dbPath;

    public FoundResourcesRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddFoundResource(FoundResource foundResource)
    {
        string query = $"INSERT INTO found_resources(" +
            $"simulation_id, " +
            $"resource_name, " +
            $"representation, " +
            $"coordinate_x, " +
            $"coordinate_y) " +
            $"VALUES(" +
            $"{foundResource.SimulationId}, " +
            $"'{foundResource.ResourceName}', " +
            $"'{foundResource.Representation}', " +
            $"{foundResource.CoordinateX}, " +
            $"{foundResource.CoordinateY})";
            ExecuteNonQuery(query);
    }

    private SqliteConnection GetConnection()
    {
        SqliteConnection sqliteConnection = new SqliteConnection($"Data Source ={_dbPath};Mode=ReadWrite");
        sqliteConnection.Open();
        return sqliteConnection;

    }

    private void ExecuteNonQuery(string query)
    {
        SqliteConnection connection = GetConnection();
        SqliteCommand command = GetCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private SqliteCommand GetCommand(string query, SqliteConnection connection)
    {
        return new SqliteCommand(query, connection);
    }
}
