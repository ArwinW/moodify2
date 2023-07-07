using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moodify.Models
{
    public class PrimaryGenres
    {
        [JsonPropertyName("music_genre_list")]
        public List<MusicGenreList> MusicGenreList { get; set; }
    }

    public class MusicGenreList
    {
        [JsonPropertyName("music_genre")]
        public MusicGenre MusicGenre { get; set; }
    }

    public class MusicGenre
    {
        [JsonPropertyName("music_genre_id")]
        public int MusicGenreId { get; set; }

        [JsonPropertyName("music_genre_parent_id")]
        public int MusicGenreParentId { get; set; }

        [JsonPropertyName("music_genre_name")]
        public string MusicGenreName { get; set; }

        [JsonPropertyName("music_genre_name_extended")]
        public string MusicGenreNameExtended { get; set; }

        [JsonPropertyName("music_genre_vanity")]
        public string MusicGenreVanity { get; set; }

        [JsonPropertyName("music_genre_list")]
        public List<MusicGenreList> RelatedTracks { get; set; } // Modify the property type to List<MusicGenreList>
    }

    public class Track
    {
        [JsonPropertyName("track_id")]
        public int TrackId { get; set; }

        [JsonPropertyName("track_name")]
        public string TrackName { get; set; }

        [JsonPropertyName("album_id")]
        public int AlbumId { get; set; }

        [JsonPropertyName("album_name")]
        public string AlbumName { get; set; }

        [JsonPropertyName("artist_id")]
        public int ArtistId { get; set; }

        [JsonPropertyName("artist_name")]
        public string ArtistName { get; set; }

        [JsonPropertyName("track_share_url")]
        public string TrackShareUrl { get; set; }

        [JsonPropertyName("track_edit_url")]
        public string TrackEditUrl { get; set; }

        [JsonPropertyName("restricted")]
        public int Restricted { get; set; }

        [JsonPropertyName("updated_time")]
        public string UpdatedTime { get; set; }

        [JsonPropertyName("primary_genres")]
        public PrimaryGenres PrimaryGenres { get; set; }

        // Additional properties for related tracks
        [JsonPropertyName("relatedTracksByGenre")]

        public List<Track> relatedTracksByGenre { get; set; }
        [JsonPropertyName("relatedTracksByArtist")]

        public List<Track> relatedTracksByArtist { get; set; }
    }

    public class TrackList
    {
        [JsonPropertyName("track")]
        public Track Track { get; set; }
    }

    public class Body
    {
        [JsonPropertyName("track_list")]
        public List<TrackList> TrackList { get; set; }
    }

    public class Header
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("execute_time")]
        public double ExecuteTime { get; set; }

        [JsonPropertyName("available")]
        public int Available { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }

        [JsonPropertyName("body")]
        public Body Body { get; set; }
    }

    public class RootObject
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }
}
