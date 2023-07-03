using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using Moodify.db;
using Org.BouncyCastle.Utilities;

namespace YourApplication.Controllers
{
    public class LogsController : Controller
    {
        private readonly Database dataAccess;

        public LogsController()
        {
            // Initialize the data access class
            dataAccess = new UserDataAccess();
        }

        // GET: Logs
        public ActionResult Index()
        {
            // Retrieve logs from the database or any other data source
            List<Log> logs = (List<Log>)GetLogsFromDatabase();

            return View(logs);
        }

        private IEnumerable<Log> GetLogsFromDatabase()
        {
            var tablename = "logs";
            IEnumerable<Log> data = dataAccess.GetAll<Log>(tablename);

            // Implement your logic to retrieve logs from the database using the dataAccess object
            // Return a list of Log objects

            List<Log> logs = new List<Log>();

            foreach (var log in data)
            {
                Console.WriteLine($"user_id: {log.user_id}, song_id: {log.song_id}, created_at: {log.created_at}");
                Log updatedLog = new Log
                {
                    user_id = log.user_id,      // Set the UserId based on the actual value in the data
                    song_id = log.song_id,      // Set the SongId based on the actual value in the data
                    created_at = log.created_at // Set the CreatedAt based on the actual value in the data
                };

                logs.Add(updatedLog);
            }

            return logs;
        }
    }
}
