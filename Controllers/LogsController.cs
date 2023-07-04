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
        private readonly DataAccess dataAccess;

        public LogsController(HttpClient httpClient)
        {
            // Initialize the data access class
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://localhost:5001/"); // Set the base URI of your API here

            // Initialize the database access class
            this.dataAccess = new DataAccess();
        }

        // GET: Logs
        public async Task<ActionResult> IndexAsync()
        {
            // Retrieve logs from the database or any other data source
            List<Log> logs = await GetLogsFromDatabase("logs");

             return View(logs);
        }

        private async Task<List<Log>> GetLogsFromDatabase(string tablename)
        {
            var response = await httpClient.GetAsync($"api/{tablename}");
            if (response.IsSuccessStatusCode)
            {
                var logsJson = await response.Content.ReadAsStringAsync();
                var logs = JsonSerializer.Deserialize<IEnumerable<Log>>(logsJson);

                // Implement your logic to retrieve logs from the database using the dataAccess object
                // Return a list of Log objects

                List<Log> logslist = new List<Log>();

                foreach (var log in logs)
                {
                    Log updatedLog = new Log
                    {
                        user_id = log.user_id,
                        song_id = log.song_id,
                        created_at = log.created_at
                    };

                    // Get the username for the current user_id
                    string username = dataAccess.GetUsernameById(log.user_id);
                    updatedLog.username = username;

                    string songname = dataAccess.GetSongById(log.song_id);
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