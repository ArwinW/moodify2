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
        private readonly DataAccess _database;

        public ApiController()
        {
            _database = new DataAccess();
        }

        [HttpGet("/api/{tableName}")]
        public IActionResult Get(string tableName)
        {
            switch (tableName)
            {
                case "logs":
                    var logs = _database.GetAll<Log>(tableName);
                    return Ok(logs);


                // Add more cases for other table names if needed

                default:
                    return BadRequest("Invalid table name");
            }
        }


        [HttpGet("/api/{tableName}")]
        public IActionResult Get(string tableName)
        {
            
                    var username = dataAccess.GetUsernameById(log.user_id);

                    return Ok(username);


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