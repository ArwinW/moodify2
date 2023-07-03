using System;

namespace Moodify.Models
{
    public class Log
    {
        public int user_id { get; set; }
        public int song_id { get; set; }
        public DateTime created_at { get; set; }
    }
}
