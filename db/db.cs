using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using System.Collections.Generic;
using System.Data;
using Moodify.Models;
using System.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using System.Linq;

namespace Moodify.db
{
    public class db : Database
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

            bool succes = false;

            string sqlStatement = "SELECT * FROM users WHERE username = @username AND password = @password";
            using (MySqlConnection connection = this.GetConnection())
            {

                MySqlCommand command = new MySqlCommand(sqlStatement, connection);


                command.Parameters.Add("@username", MySqlDbType.VarChar, 255).Value = user.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar, 255).Value = user.Password;

                try
                {
                    string query = $"SELECT * FROM user WHERE username = '{username}'";
                    IEnumerable<UserModel> user = this.ExecuteQuery<UserModel>(query);
                    return user.FirstOrDefault();
                }
                catch
                {

                }


            }
        }
    }
}

