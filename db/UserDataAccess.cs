﻿using Moodify.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moodify.db
{
    public class UserDataAccess : Database
    {
        public UserModel GetUserByUsername(string username)
        {
            string query = $"SELECT * FROM user WHERE username = '{username}'";
            IEnumerable<UserModel> user = this.ExecuteQuery<UserModel>(query);
            return user.FirstOrDefault();
        }


        public void InsertData()
        {
            using (MySqlConnection connection = base.GetConnection())
            {
                // Use the connection to execute database operations
                // For example:
                string query = "INSERT INTO TableName (Column1, Column2) VALUES (@Value1, @Value2)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Set parameter values and execute the command
                }
            }
        }

        public List<Log> GetLogs()
        {
            List<Log> logs = new List<Log>();
            using (MySqlConnection connection = base.GetConnection())
            {
                string query = "SELECT * FROM logs";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Execute the command and retrieve the logs from the database
                    // Process the retrieved data and populate the logs list
                }
            }

            return logs; // Return the populated list of Log objects
        }
    }
}