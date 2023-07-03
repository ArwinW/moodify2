using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using System.Collections.Generic;

namespace Moodify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult<SongModel> Get(string songTitle, string artistName)
        {
            // Logic to retrieve search results based on the provided parameters
            // Use songTitle and artistName to query your data source (e.g., database) and get the matching results

            var searchResults = new SongModel { Id = 1, Description = "testD" , Name = "TestName"};// Perform the search and retrieve the matching results

        return Ok(searchResults); // Return the search results as JSON
        }


        [HttpPost]
        public IActionResult Post([FromBody] UserModel model)
        {
            // Logic to process the posted data
            // Example: Save the data to the database

            return Created("api/yourcontroller", model); // Return a 201 Created response with the saved data
        }

        // Other API endpoints and actions as needed

        // Example of an endpoint with route parameters
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Logic to retrieve data based on the provided ID
            var data = 1;// Retrieve data from your source based on ID (e.g., databa1qse query)

            if (data == null)
            {
                return NotFound(); // Return a 404 Not Found response if the data doesn't exist
            }

            return Ok(data); // Return the data as JSON
        }
    }
}