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
using Moodify.Models;
using System.Linq;

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
        public async Task<ActionResult<List<Track>>> Get(string songTitle, string artistName)
        {
            // Prepare the API request URL with the required parameters
            string requestUrl = $"track.search?q_track={Uri.EscapeDataString(songTitle)}&q_artist={Uri.EscapeDataString(artistName)}&apikey=151f5eaedfddb9c774a269dfe70ff766&f_has_lyrics=true&s_track_rating=desc&page_size=10";

            // Send the API request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            // Process the API response
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as JSON
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<RootObject>(responseContent);

                var trackList = responseObject.Message.Body.TrackList.Select(t => t.Track).ToList();

                var songModels = trackList.Select(track => new Track
                {
                    TrackId = track.TrackId,
                    TrackName = track.TrackName,
                    AlbumId = track.AlbumId,
                    AlbumName = track.AlbumName,
                    ArtistId = track.ArtistId,
                    ArtistName = track.ArtistName,
                    TrackShareUrl = track.TrackShareUrl,
                    TrackEditUrl = track.TrackEditUrl,
                    PrimaryGenres = new PrimaryGenres
                    {
                        MusicGenreList = track.PrimaryGenres?.MusicGenreList ?? new List<MusicGenreList>()
                    },
                }).ToList();

                // Fetch related tracks by genre and artist for each track
                foreach (var track in songModels)
                {
                    var genreId = track.PrimaryGenres.MusicGenreList.FirstOrDefault()?.MusicGenre.MusicGenreId;
                    var artistId = track.ArtistId;

                    // Fetch 10 tracks with the same genre
                    var relatedTracksByGenre = await GetRelatedTracksByGenre(genreId, 10);
                    track.relatedTracksByGenre = relatedTracksByGenre;

                    // Fetch 10 tracks with the same artist
                    var relatedTracksByArtist = await GetRelatedTracksByArtist(artistId, 10);
                    track.relatedTracksByArtist = relatedTracksByArtist;


                    // Now each track in songModels will have the related tracks by genre and artist

                    // Access the related tracks from the Musixmatch API response

                    // Assign the related tracks to the corresponding track in songModels
                    var correspondingTrack = songModels.FirstOrDefault(t => t.TrackId == track.TrackId);
                    if (correspondingTrack != null)
                    {
                        correspondingTrack.relatedTracksByGenre = relatedTracksByGenre;
                        correspondingTrack.relatedTracksByArtist = relatedTracksByArtist;
                    }
                }

                _database.InsertLog(songModels.TrackId, HttpContext);
            }

            // Return an empty list if the API request was not successful
            return new List<Track>();
        }

        private async Task<List<Track>> GetRelatedTracksByGenre(int? genreId, int pageSize)
        {
            // Prepare the API request URL with the required parameters
            string requestUrl = $"track.search?f_music_genre_id={genreId}&s_track_rating=desc&page_size={pageSize}&apikey=151f5eaedfddb9c774a269dfe70ff766";

            // Send the API request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            // Process the API response
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as JSON
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<RootObject>(responseContent);

                var trackList = responseObject.Message.Body.TrackList.Select(t => t.Track).ToList();

                return trackList;
            }

            // Return an empty list if the API request was not successful
            return new List<Track>();
        }

        private async Task<List<Track>> GetRelatedTracksByArtist(int artistId, int pageSize)
        {
            // Prepare the API request URL with the required parameters
            string requestUrl = $"track.search?f_artist_id={artistId}&s_track_rating=desc&page_size={pageSize}&apikey=151f5eaedfddb9c774a269dfe70ff766";

            // Send the API request
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            // Process the API response
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as JSON
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<RootObject>(responseContent);

                var trackList = responseObject.Message.Body.TrackList.Select(t => t.Track).ToList();

                return trackList;
            }

            // Return an empty list if the API request was not successful
            return new List<Track>();
        }
    }
}