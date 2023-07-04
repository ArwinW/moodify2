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
            List<Log> logs = await GetLogsFromDatabase("logs");

            return View(logs);
        }

        private async Task<List<Log>> GetLogsFromDatabase(string tableName)
        {
            var response = await httpClient.GetAsync($"api/{tableName}");
            if (response.IsSuccessStatusCode)
            {
                var logsJson = await response.Content.ReadAsStringAsync();
                var logs = JsonSerializer.Deserialize<IEnumerable<Log>>(logsJson);

                List<Log> logslist = new List<Log>();

                foreach (var log in logs)
                {
                    Console.WriteLine($"user_id: {log.user_id}, song_id: {log.song_id}, created_at: {log.created_at}");
                    Log updatedLog = new Log
                    {
                        user_id = log.user_id,      // Set the UserId based on the actual value in the data
                        song_id = log.song_id,      // Set the SongId based on the actual value in the data
                        created_at = log.created_at // Set the CreatedAt based on the actual value in the data
                    };

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
