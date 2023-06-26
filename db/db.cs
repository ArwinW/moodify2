using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using System.Collections.Generic;
using System.Data;

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
            string connectionString = configuration.GetConnectionString("Default");
            return new MySqlConnection(connectionString);
        }
    }
}
