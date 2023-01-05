using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using Microsoft.Data.Sqlite;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository;

public class ColonisationSummaryRepository : IColonisationSummaryRepository
{
    private readonly string _dbPath;

    public ColonisationSummaryRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddRoverSummary(RoverSummary roverSummary)
    {
        string tableName = "rovers";
        string query = $"INSERT INTO {tableName}(" +
            $"rover_id," +
            $")" +
            $"VALUES(" +
            $"'{roverSummary.RoverId}')";
        ExecuteNonQuery(query);

        foreach (var resourceSummary in roverSummary.Resources)
        {
            AddResourceSummary(tableName, resourceSummary);
        }
    }

    public void AddCommandCenterSummary(CommandCenterSummary commandCenterSummary)
    {
        string tableName = "command_centers";
        string query = $"INSERT INTO {tableName}(" +
            $"command_center_id," +
            $")" +
            $"VALUES(" +
            $"'{commandCenterSummary.CommandCenterId}')";
        ExecuteNonQuery(query);

        foreach (var resourceSummary in commandCenterSummary.Resources)
        {
            AddResourceSummary(tableName, resourceSummary);
        }
    }

    public void AddResourceSummary(string target, ResourceSummary resourceSummary)
    {
        // int id auto generated
        string query = $"INSERT INTO {target}_resources(" +
            $"object_id," +
            $"resource_type," +
            $"collected_amount" +
            $")" +
            $"VALUES(" +
            $"'{resourceSummary.ObjectId}'," +
            $"'{resourceSummary.ResourceType}'," +
            $"{resourceSummary.CollectedAmount}" +
            $")";
        ExecuteNonQuery(query);
    }

    public void AddConstructionSummary(ConstructionSummary constructionSummary)
    {
        string tableName = "constructions";
        // int construction_id auto generated
        string query = $"INSERT INTO {tableName}(" +
            $"constructed_object_id," +
            $"constructor_object_id," +
            $")" +
            $"VALUES(" +
            $"'{constructionSummary.ConstructedObjectId}'," +
            $"'{constructionSummary.ConstructorObjectId}')";
        ExecuteNonQuery(query);

        int constructionId = GetLastConstructionId();
        foreach (var materialSummary in constructionSummary.Resources)
        {
            materialSummary.ConstructionSummaryId = constructionId;
        }

        foreach (var materialSummary in constructionSummary.Resources)
        {
            AddConstructionMaterialsSummary(tableName, materialSummary);
        }
    }

    private int GetLastConstructionId()
    {
        string query = "SELECT construction_id FROM table ORDER BY construction_id DESC LIMIT 1;";
        SqliteConnection connection = GetConnection();
        SqliteCommand command = GetCommand(query, connection);
        using SqliteDataReader sqliteDataReader = command.ExecuteReader();
        sqliteDataReader.Read();
        return sqliteDataReader.GetInt32(0);
    }

    public void AddConstructionMaterialsSummary(string target, ConstructionMaterialsSummary constructionMaterialsSummary)
    {
        // int id auto generated
        string query = $"INSERT INTO {target}_materials(" +
            $"construction_id," +
            $"resource_type," +
            $"used_amount" +
            $")" +
            $"VALUES(" +
            $"'{constructionMaterialsSummary.ConstructionSummaryId}'," +
            $"'{constructionMaterialsSummary.ResourceType}'," +
            $"{constructionMaterialsSummary.UsedAmount}" +
            $")";
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

    public void ResetCounterTypes()
    {
        string query1 = $"DELETE FROM rovers";
        string query2 = $"DELETE FROM rovers_resources";
        string query3 = $"DELETE FROM command_centers";
        string query4 = $"DELETE FROM command_centers_resources";

        ExecuteNonQuery(query1);
        ExecuteNonQuery(query2);
        ExecuteNonQuery(query3);
        ExecuteNonQuery(query4);
    }

    public void ResetAll()
    {
        string query1 = $"DELETE FROM rovers";
        string query2 = $"DELETE FROM rovers_resources";
        string query3 = $"DELETE FROM command_centers";
        string query4 = $"DELETE FROM command_centers_resources";
        string query5 = $"DELETE FROM constructions";
        string query6 = $"DELETE FROM constructions_resources";

        ExecuteNonQuery(query1);
        ExecuteNonQuery(query2);
        ExecuteNonQuery(query3);
        ExecuteNonQuery(query4);
        ExecuteNonQuery(query5);
        ExecuteNonQuery(query6);
    }
}
