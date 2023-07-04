using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Moodify.db
{
    public class Database
    {
        public MySqlConnection GetConnection()
        {
            // string connectionString = configuration.GetConnectionString("Default");
            // todo: Get the connection string from config file

            return new MySqlConnection("Server=localhost;User=root;Password=;Database=moodify;Convert Zero Datetime=True");
        }

        public IEnumerable<T> GetAll<T>(string tablename)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string query = $"SELECT * FROM {tablename}";
                return connection.Query<T>(query);
            }
        }

        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.Query<T>(query);
            }
        }

        public int ExecuteCommand(string query)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.Execute(query);
            }
        }

        public string GetUsernameById(int user_id)
        {
            string query = $"SELECT username FROM users WHERE id = {user_id}";

            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<string>(query);
            }
        }

        public string GetSongById(int song_id)
    {
        string query = $"SELECT name FROM songs WHERE id = {song_id}";

        using (IDbConnection connection = GetConnection())
        {
            connection.Open();
            return connection.QueryFirstOrDefault<string>(query);
        }
    }
    }
}
