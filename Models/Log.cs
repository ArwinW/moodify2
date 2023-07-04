using System;
using System.Text.Json.Serialization;

namespace Moodify.Models
{
    public class Log
    {
        [JsonPropertyName("user_id")]

        public int user_id { get; set; }
        [JsonPropertyName("song_id")]

        public int song_id { get; set; }
        [JsonPropertyName("created_at")]

        public DateTime created_at { get; set; }
    }
}
