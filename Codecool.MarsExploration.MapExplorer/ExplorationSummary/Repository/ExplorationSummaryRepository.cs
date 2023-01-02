using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codecool.MarsExploration.MapExplorer.ExplorationSummary.Model;
using System.Data;

namespace Codecool.MarsExploration.MapExplorer.ExplorationSummary.Repository
{
    public class ExplorationSummaryRepository : IExplorationSummaryRepository
    {
        private readonly string _dbPath;

        public ExplorationSummaryRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void AddExplorationSummary(Model.ExplorationSummary explorationSummary)
        {
            string query = $"INSERT INTO simulations(" +
                $"timestamp, " +
                $"logfile_path, " +
                $"number_of_steps, " +
                $"found_resources, " +
                $"outcome) " +
                $"VALUES(" +
                $"{explorationSummary.TimeStamp}, " +
                $"'{explorationSummary.LogfilePath}', " +
                $"{explorationSummary.NumberOfSteps}, " +
                $"'{explorationSummary.FoundResources}', " +
                $"'{explorationSummary.Outcome}')";
            ExecuteNonQuery(query);
        }

        public int GetExplorationSummaryIdByTimeStamp(long timestamp)
        {
            string query = $"SELECT id FROM simulations WHERE timestamp={timestamp}";
            SqliteConnection connection = GetConnection();
            SqliteCommand command = GetCommand(query, connection);
            using SqliteDataReader sqliteDataReader = command.ExecuteReader();    
            sqliteDataReader.Read();
            return sqliteDataReader.GetInt32(0);
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
}
