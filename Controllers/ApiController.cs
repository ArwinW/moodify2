using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using System.Collections.Generic;
using Moodify.db;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Moodify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly DataAccess _database;
        private readonly HttpClient _httpClient;

        public ApiController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.musixmatch.com/ws/1.1/");
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

        [HttpGet]
        public async Task<ActionResult<Track>> Get(string songTitle, string artistName)
        {
            // Prepare the API request URL with the required parameters
            string requestUrl = $"track.search?q_track={Uri.EscapeDataString(songTitle)}&q_artist={Uri.EscapeDataString(artistName)}&apikey=151f5eaedfddb9c774a269dfe70ff766";

            // Send the API request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            // Process the API response
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as JSON
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<RootObject>(responseContent);

                var trackList = responseObject.Message.Body.TrackList;
                var songModels = new List<Track>();

                foreach (var track in trackList)
                {
                    var songModel = new Track
                    {
                        TrackId = track.Track.TrackId,
                        TrackName = track.Track.TrackName,
                        AlbumId = track.Track.AlbumId,
                        AlbumName = track.Track.AlbumName,
                        ArtistId = track.Track.ArtistId,
                        ArtistName = track.Track.ArtistName,
                        TrackShareUrl = track.Track.TrackShareUrl,
                        TrackEditUrl = track.Track.TrackEditUrl,
                        PrimaryGenres = track.Track.PrimaryGenres
                    };

                    songModels.Add(songModel);
                }

                return Ok(songModels); // Return the search results as JSON
            }
            else
            {
                // Handle the case when the API request is not successful
                return BadRequest(); // or handle the error case accordingly
            }
        }

    }
}