using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using System.Collections.Generic;
using System.Data;
using Moodify.Models;
using System.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;

namespace Moodify.db
{
    public class db
    {
        private readonly IConfiguration configuration;

        public db(IConfiguration config)
        {
            configuration = config;
        }

        public MySqlConnection GetConnection()
        {
            // todo: hier later de connection string ophalen uit de app.settings.
            //string connectionString = configuration.GetConnectionString("Default");

            // onderstaand de connectiostring nog invullen.
            string connectionString = "Server=localhost; User ID=root; Database=moodify";

            return new MySqlConnection(connectionString);
        }

        public bool FindUserByUsernameAndPassword(UserModel user)
        {

            using (MySqlConnection connection = this.GetConnection())
            {
                string sql = "SELECT * FROM users WHERE username = @username AND password = @password";

                using (MySqlCommand cmd = new(sql, connection))
                {
                    connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                    }
                }
            }
        }
    }
}
