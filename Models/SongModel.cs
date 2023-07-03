using System.Text.Json.Serialization;

namespace Moodify.Models
{
    public class SongModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

    }
}
