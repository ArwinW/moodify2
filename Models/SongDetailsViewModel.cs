using Moodify.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class SongDetailsViewModel
{
    public Track Song { get; set; }
    // Additional properties for related tracks
    [JsonPropertyName("relatedTracksByGenre")]

    public List<string> relatedTracksByGenre { get; set; }
    [JsonPropertyName("relatedTracksByArtist")]

    public List<string> relatedTracksByArtist { get; set; }
}
