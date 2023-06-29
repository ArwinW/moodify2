using System;

namespace Moodify.Models
{
    public class Log
    {
        public int UserId { get; set; }
        public int SongId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
