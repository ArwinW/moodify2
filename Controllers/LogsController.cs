using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using Moodify.db;
using Org.BouncyCastle.Utilities;
using System.Net.Http;

using System.Threading.Tasks;
using System.Text.Json;

namespace Moodify.Controllers
{
    public class LogsController : Controller
    {
        private readonly HttpClient httpClient;

        public LogsController(HttpClient httpClient)
        {
            // Initialize the data access class
            this.httpClient = httpClient;
            httpClient.BaseAddress = new Uri("https://localhost:5001/"); // Set the base URI of your API here
        }

        // GET: Logs
        public async Task<ActionResult> IndexAsync()
        {
            // Retrieve logs from the database or any other data source
            List<Log> logs = await GetLogsFromDatabase();

            return View(logs);
        }

        private IEnumerable<Log> GetLogsFromDatabase()
        {
            var tablename = "logs";
            IEnumerable<Log> data = dataAccess.GetAll<Log>(tablename);

            // Implement your logic to retrieve logs from the database using the dataAccess object
            // Return a list of Log objects

            List<Log> logslist = new List<Log>();

        foreach (var log in data)
        {
            Log updatedLog = new Log
            {
                user_id = log.user_id,
                song_id = log.song_id,
                created_at = log.created_at
            };

            // Get the username for the current user_id
            string username = GetUsernameById(log.user_id);
            updatedLog.username = username;

            string songname = GetSongById(log.song_id);
            updatedLog.songname = songname;

            logslist.Add(updatedLog);
        }

            return logslist;
        }
            else
            {
                // Handle the case when the API request is not successful
                // For example, log the error or return an empty collection
                return new List<Log>(); // or handle the error case accordingly
            }
    }
}
}