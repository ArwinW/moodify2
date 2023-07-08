using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moodify.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly HttpClient _httpClient;
    private List<Track> songModels; // Add this line to declare the songModels list
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("MyApi");
        _httpContextAccessor = httpContextAccessor;

    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> Index(string songTitle, string artistName) // Update the method name to "Index"
    {
        HttpContext context = _httpContextAccessor.HttpContext;
        ISession session = context.Session;

        // Set the session value
        int? userId = session.GetInt32("UserId");
        // Logic to process the search parameters and retrieve search results 
        var response = await _httpClient.GetAsync($"/api/Api?songTitle={songTitle}&artistName={artistName}&userId={userId}");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();

            if (jsonString.StartsWith("["))
            {
                // The JSON response is an array of SongModel
                songModels = System.Text.Json.JsonSerializer.Deserialize<List<Track>>(jsonString); // Assign the deserialized list to the songModels field
                ViewData["SongModels"] = songModels;
                return View("Search", songModels);

            }
            else
            {
                // The JSON response is a single SongModel
                var songModel = System.Text.Json.JsonSerializer.Deserialize<Track>(jsonString);
                songModels = new List<Track> { songModel }; // Assign the single songModel to the songModels field as a list
                ViewData["SongModels"] = songModels;
                return View("Search", songModels);

            }
        }
        else
        {
            // Handle other error scenarios
            // Return an appropriate response or redirect to an error page

            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public IActionResult Details(int id, string name, string albumName, string artist, string genre, List<string> relatedTracksByGenre, List<string> relatedTracksByArtist)
    {
        {
            // Create a song model based on the provided parameters`
            var song = new Track
            {
                TrackId = id,
                TrackName = name,
                AlbumName = albumName,
                ArtistName = artist,
                PrimaryGenres = new PrimaryGenres()
                {
                    MusicGenreList = new List<MusicGenreList>
                {
                    new MusicGenreList
                    {
                        MusicGenre = new MusicGenre
                        {
                            MusicGenreName = genre
                        }
                    }
                }
                },
            };

            // Get related songs of the same genre, mood, and artist

            // Create a view model to pass the song details and related songs to the view
            var viewModel = new SongDetailsViewModel
            {
                Song = song,
                relatedTracksByGenre = relatedTracksByGenre,
                relatedTracksByArtist =relatedTracksByArtist
            };

            return View(viewModel);
        }

    }
}
