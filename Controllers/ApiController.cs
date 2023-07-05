using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using System.Collections.Generic;
using Moodify.db;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text.Json;

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
                var responseContent = "{\"message\":{\"header\":{\"status_code\":200,\"execute_time\":0.083615064620972,\"available\":6},\"body\":{\"track_list\":[{\"track\":{\"track_id\":66322876,\"track_name\":\"Thriller\",\"track_name_translation_list\":[],\"track_rating\":3,\"commontrack_id\":35209178,\"instrumental\":0,\"explicit\":1,\"has_lyrics\":1,\"has_subtitles\":1,\"has_richsync\":0,\"num_favourite\":0,\"album_id\":18698434,\"album_name\":\"We Are The Champions Electronic Dance Hits 2012\",\"artist_id\":24443613,\"artist_name\":\"MJ\",\"track_share_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/MJ-3\\/Thriller?utm_source=application&utm_campaign=api&utm_medium=\",\"track_edit_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/MJ-3\\/Thriller\\/edit?utm_source=application&utm_campaign=api&utm_medium=\",\"restricted\":0,\"updated_time\":\"2014-05-21T14:45:59Z\",\"primary_genres\":{\"music_genre_list\":[{\"music_genre\":{\"music_genre_id\":17,\"music_genre_parent_id\":34,\"music_genre_name\":\"Dance\",\"music_genre_name_extended\":\"Dance\",\"music_genre_vanity\":\"Dance\"}}]}}},{\"track\":{\"track_id\":105875939,\"track_name\":\"Halloween Secret Weapon 1 - Thriller\",\"track_name_translation_list\":[],\"track_rating\":1,\"commontrack_id\":58098822,\"instrumental\":0,\"explicit\":0,\"has_lyrics\":0,\"has_subtitles\":0,\"has_richsync\":0,\"num_favourite\":0,\"album_id\":22724433,\"album_name\":\"Halloween Secret Weapon 1\",\"artist_id\":29067949,\"artist_name\":\"MJ\",\"track_share_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/MJ-11\\/Halloween-Secret-Weapon-1-Thriller?utm_source=application&utm_campaign=api&utm_medium=\",\"track_edit_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/MJ-11\\/Halloween-Secret-Weapon-1-Thriller\\/edit?utm_source=application&utm_campaign=api&utm_medium=\",\"restricted\":0,\"updated_time\":\"2016-03-16T05:23:40Z\",\"primary_genres\":{\"music_genre_list\":[]}}},{\"track\":{\"track_id\":109898681,\"track_name\":\"Thriller Hunter\",\"track_name_translation_list\":[],\"track_rating\":1,\"commontrack_id\":60307977,\"instrumental\":0,\"explicit\":0,\"has_lyrics\":0,\"has_subtitles\":0,\"has_richsync\":0,\"num_favourite\":0,\"album_id\":23296504,\"album_name\":\"THIS IS MASH UP! in memory of MICHAEL JACKSON\",\"artist_id\":31693710,\"artist_name\":\"2 Many MJ's\",\"track_share_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/2-Many-MJ-s\\/Thriller-Hunter?utm_source=application&utm_campaign=api&utm_medium=\",\"track_edit_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/2-Many-MJ-s\\/Thriller-Hunter\\/edit?utm_source=application&utm_campaign=api&utm_medium=\",\"restricted\":0,\"updated_time\":\"2016-04-21T10:05:05Z\",\"primary_genres\":{\"music_genre_list\":[]}}},{\"track\":{\"track_id\":56954895,\"track_name\":\"Thriller \\/ Hunter\",\"track_name_translation_list\":[],\"track_rating\":1,\"commontrack_id\":28088334,\"instrumental\":0,\"explicit\":0,\"has_lyrics\":0,\"has_subtitles\":0,\"has_richsync\":0,\"num_favourite\":0,\"album_id\":17460586,\"album_name\":\"This Is Mash Up - IN MEMORY OF MICHAEL JACKSON\",\"artist_id\":26648341,\"artist_name\":\"2 Many MJ’s\",\"track_share_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/2-Many-MJ%E2%80%99s\\/Thriller-Hunter?utm_source=application&utm_campaign=api&utm_medium=\",\"track_edit_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/2-Many-MJ%E2%80%99s\\/Thriller-Hunter\\/edit?utm_source=application&utm_campaign=api&utm_medium=\",\"restricted\":0,\"updated_time\":\"2014-02-23T06:04:09Z\",\"primary_genres\":{\"music_genre_list\":[]}}},{\"track\":{\"track_id\":65027507,\"track_name\":\"Halloween Secret Weapon 1 (Thriller)\",\"track_name_translation_list\":[],\"track_rating\":1,\"commontrack_id\":34248677,\"instrumental\":0,\"explicit\":0,\"has_lyrics\":0,\"has_subtitles\":0,\"has_richsync\":0,\"num_favourite\":0,\"album_id\":18512728,\"album_name\":\"Halloween Secret Weapon 1 (Thriller)\",\"artist_id\":24443613,\"artist_name\":\"MJ\",\"track_share_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/MJ-3\\/Halloween-Secret-Weapon-1-Thriller?utm_source=application&utm_campaign=api&utm_medium=\",\"track_edit_url\":\"https:\\/\\/www.musixmatch.com\\/lyrics\\/MJ-3\\/Halloween-Secret-Weapon-1-Thriller\\/edit?utm_source=application&utm_campaign=api&utm_medium=\",\"restricted\":0,\"updated_time\":\"2014-03-07T15:37:05Z\",\"primary_genres\":{\"music_genre_list\":[]}}},{\"track\":{\"track_id\":231440695,\"track_name\":\"Para-Social Psycho-Sexual Erotic Thr\"}}]}}}";
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