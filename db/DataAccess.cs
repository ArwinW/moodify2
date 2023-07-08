using Dapper;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moodify.Models;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Moodify.db
{
    public class DataAccess
    {
        public MySqlConnection GetConnection()
        {
            // string connectionString = configuration.GetConnectionString("Default");
            // todo: Get the connection string from config file

            return new MySqlConnection("Server=localhost;User=root;Password=;Database=moodify;Convert Zero Datetime=True");
        }

        public IEnumerable<T> GetAll<T>(string tablename)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                string query = $"SELECT * FROM {tablename}";
                return connection.Query<T>(query);
            }
        }

        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.Query<T>(query);
            }
        }

        public int ExecuteCommand(string query)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.Execute(query);
            }
        }
        public UserModel GetUserByUsernameAndPassword(string username, string password)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT * FROM users WHERE username = @Username AND password = @Password";
                return connection.QuerySingleOrDefault<UserModel>(sql, new { Username = username, Password = password });
            }
        }


        public string GetUsernameById(int user_id)
        {
            string query = $"SELECT username FROM users WHERE id = {user_id}";

            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<string>(query);
            }
        }

        public string GetSongById(int song_id)
        {
            string query = $"SELECT name FROM songs WHERE id = {song_id}";

            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<string>(query);
            }
        }
        public int InsertUser(UserModel userModel)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                var sql = "INSERT INTO users (username, password) VALUES (@Username, @Password)";
                return connection.Execute(sql, userModel);
            }
        }

        public bool IsUsernameTaken(string username)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT COUNT(*) FROM users WHERE username = @Username";
                int count = connection.ExecuteScalar<int>(sql, new { Username = username });
                return count > 0;
            }
        }

        public void InsertLog(int songId, HttpContext httpContext)
        {
            int? userId = httpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    var sql = "INSERT INTO logs (user_id, song_id) VALUES (@UserId, @SongId)";
                    connection.Execute(sql, new { UserId = userId.Value, SongId = songId });
                }
            }

            // Other controller methods...
        }
        public void InsertSongs(List<Track> songModels)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var songModel in songModels)
                        {
                            // Insert the artist if it doesn't exist
                            var artistSql = @"INSERT INTO artists (name)
                                      SELECT @ArtistName
                                      FROM dual
                                      WHERE NOT EXISTS (
                                          SELECT 1 FROM artists WHERE name = @ArtistName
                                      );";
                            var artistParams = new { ArtistName = songModel.ArtistName ?? "onbekend" };
                            connection.Execute(artistSql, artistParams);

                            // Get the artist ID
                            var artistIdSql = "SELECT id FROM artists WHERE name = @ArtistName;";
                            var artistId = connection.ExecuteScalar<int>(artistIdSql, artistParams);

                            // Insert the album if it doesn't exist
                            var albumSql = @"INSERT INTO album (name)
                                     SELECT @Name
                                     FROM dual
                                     WHERE NOT EXISTS (
                                         SELECT 1 FROM album WHERE name = @Name
                                     );";
                            var albumParams = new { Name = songModel.AlbumName ?? "onbekend" };
                            connection.Execute(albumSql, albumParams);

                            // Get the album ID
                            var albumIdSql = "SELECT id FROM album WHERE name = @Name;";
                            var albumId = connection.ExecuteScalar<int>(albumIdSql, albumParams);

                            // Insert the genre if it doesn't exist
                            var genreSql = @"INSERT INTO genre (naam)
                                     SELECT @GenreName
                                     FROM dual
                                     WHERE NOT EXISTS (
                                         SELECT 1 FROM genre WHERE naam = @GenreName
                                     );";
                            var genreParams = new { GenreName = songModel.PrimaryGenres?.MusicGenreList?.FirstOrDefault()?.MusicGenre?.MusicGenreName ?? "onbekend" };
                            connection.Execute(genreSql, genreParams);

                            // Get the genre ID
                            var genreIdSql = "SELECT id FROM genre WHERE naam = @GenreName;";
                            var genreId = connection.ExecuteScalar<int>(genreIdSql, genreParams);

                            // Insert the song record
                            var songSql = @"INSERT INTO songs (name, artist_id, album_id, genre_id) 
                                    SELECT @Name, @ArtistId, @AlbumId, @GenreId
                                    FROM dual
                                    WHERE NOT EXISTS (
                                        SELECT 1 FROM songs WHERE name = @Name
                                    );";

                            var songParams = new
                            {
                                Name = songModel.TrackName,
                                ArtistId = artistId,
                                AlbumId = albumId,
                                GenreId = genreId
                            };

                            connection.Execute(songSql, songParams, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public List<string> GetAllSongTitles()
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                var sql = "SELECT name FROM songs";
                return connection.Query<string>(sql).ToList();
            }
        }
        public void InsertLog(int? userId, string songTitle, DateTime currentTime)
        {
            using (IDbConnection connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var logSql = @"INSERT INTO logs (user_id, song_title, created_at) 
                                    SELECT @user_id, @song_title, @created_at
                                    FROM dual";
                        var logParams = new
                        {
                            user_id = userId,
                            song_title = songTitle,
                            created_at = currentTime

                        };
                        connection.Execute(logSql, logParams);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }


        }

    }
}
