using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using System.Collections.Generic;
using Moodify.db;


namespace Moodify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly Database _database;

        public ApiController()
        {
            _database = new Database();
        }

        [HttpGet("/api/logs")]
        public IActionResult Get()
        {
            var logs = _database.GetAll<Log>("logs");

            return Ok(logs);
        }

        [HttpGet]
        public ActionResult<SongModel> Get(string songTitle, string artistName)
        {
            // Logic to retrieve search results based on the provided parameters
            // Use songTitle and artistName to query your data source (e.g., database) and get the matching results

            var searchResults = new SongModel { Id = 1, Description = "testD", Name = "TestName" };// Perform the search and retrieve the matching results

            return Ok(searchResults); // Return the search results as JSON
        }
    }
}