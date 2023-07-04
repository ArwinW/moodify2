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
                
            }
            return View(logs);
        }

    private IEnumerable<Log> GetLogsFromDatabase()
    {
        var tablename = "logs";
        IEnumerable<Log> data = dataAccess.GetAll<Log>(tablename);

        List<Log> logs = new List<Log>();

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

            logs.Add(updatedLog);
        }

        return logs;
    }
}
