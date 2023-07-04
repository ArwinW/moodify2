using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moodify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using Amazon.Runtime;
using System.Net;

namespace Moodify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("MyApi");
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
        public async Task<IActionResult> index(string songTitle, string artistName)
        {
            // Logic to p       rocess the search parameters and retrieve search results
            var response = await _httpClient.GetAsync($"/api/Api?songTitle={songTitle}&artistName={artistName}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString.StartsWith("["))
                {
                    // The JSON response is an array of SongModel
                    var songModels = JsonSerializer.Deserialize<List<SongModel>>(jsonString);
                    return View("Search", songModels);
                }
                else
                {
                    // The JSON response is a single SongModel
                    var songModel = JsonSerializer.Deserialize<SongModel>(jsonString);
                    var songModels = new List<SongModel> { songModel };
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


    }

}
