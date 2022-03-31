using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MovieDB.Api.App.Entities;

namespace MovieDB.Importer
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string ImdbIdentifier { get; set; }
        public MovieGenre Genre { get; set; }
        public Rating Rating { get; set; }
        public string? PosterUrl { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new(254);

            builder.AppendFormat("Title: {0}", Title ?? "Unknown Title")
                .AppendLine()
                .AppendFormat("Date: {0}", SeenAt)
                .AppendLine()
                .AppendFormat("Imdb: {0}", ImdbIdentifier ?? "No Imdb ID")
                .AppendLine()
                .AppendFormat("Genre: {0}", Genre)
                .AppendLine()
                .AppendFormat("Rating: {0}", Rating)
                .AppendLine()
                .AppendFormat("Created: {0}", CreatedAt)
                .AppendLine()
                .AppendFormat("Updated: {0}", UpdatedAt);

            return builder.ToString();
        }
    }

    public class Concert
    {
        public int Id { get; set; }
        public string Artist { get; set; }
        public DateTime SeenAt { get; set; }
        public string Location { get; set; }
        public ConcertGenre Genre { get; set; }
        public Rating Rating { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class Theater
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SeenAt { get; set; }
        public string Location { get; set; }
        public TheaterGenre Genre { get; set; }
        public Rating Rating { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    class Program
    {

        static async Task Main(string[] args)
        {
            AppDbContext db = new AppDbContext();
            // await ImportMovies(db);
            // await ImportConcerts(db);
            // await ImportTheaters(db);

            await GetPosterUrls(db);
        }

        private static async Task ImportMovies(AppDbContext db)
        {
            List<Movie> movies = new();
            int count = 0;
            string fileName = "Data/col_movies.bson";
            using var stream = File.OpenRead(fileName);
            using var reader = new BsonBinaryReader(stream);

            while (!reader.IsAtEndOfFile())
            {
                Movie movie = new();

                reader.ReadStartDocument();
                string _i = reader.ReadName();
                ObjectId _idValue = reader.ReadObjectId();

                string _t = reader.ReadName();
                string titleValue = reader.ReadString();
                movie.Title = titleValue;

                string _d = reader.ReadName();
                long dateValue = reader.ReadDateTime();
                var date = DateTimeOffset.FromUnixTimeMilliseconds(dateValue);
                movie.SeenAt = date.DateTime;

                string _im = reader.ReadName();
                string imdbId = reader.ReadString();
                movie.ImdbIdentifier = imdbId;

                string _g = reader.ReadName();
                reader.ReadStartDocument();
                string _gi = reader.ReadName();
                string genreIdentifierValue = reader.ReadString();
                Console.WriteLine(genreIdentifierValue);
                movie.Genre = ToMovieGenre(genreIdentifierValue);
                string _x = reader.ReadName();
                string _c = reader.ReadString();
                reader.ReadEndDocument();

                if (count >= 7)
                {
                    string _b = reader.ReadName();
                    string ratingValue = reader.ReadString();
                    movie.Rating = ToRating(ratingValue);
                }

                string _k = reader.ReadName();
                ObjectId _l = reader.ReadObjectId();

                string _s = reader.ReadName();
                long createdValue = reader.ReadDateTime();
                var createdDate = DateTimeOffset.FromUnixTimeMilliseconds(createdValue);
                movie.CreatedAt = createdDate.UtcDateTime;

                string _y = reader.ReadName();
                long updatedValue = reader.ReadDateTime();
                var updatedDate = DateTimeOffset.FromUnixTimeMilliseconds(updatedValue);
                movie.UpdatedAt = updatedDate.UtcDateTime;

                string _q = reader.ReadName();
                if (reader.CurrentBsonType is BsonType.DateTime)
                {
                    long deletedValue = reader.ReadDateTime();
                    DateTimeOffset deletedDate = DateTimeOffset.FromUnixTimeMilliseconds(deletedValue);
                    movie.DeletedAt = deletedDate.UtcDateTime;
                }
                else
                {
                    reader.SkipValue();
                }

                if (count < 7)
                {
                    string _ = reader.ReadName();
                    string ratingValue = reader.ReadString();
                    movie.Rating = ToRating(ratingValue);
                }

                reader.ReadEndDocument();

                movie.AccountId = 1;

                movies.Add(movie);

                count++;
            }

            Console.WriteLine(count);
            Console.WriteLine(movies.Count);

            reader.Close();
            stream.Close();

            Console.WriteLine(movies[0]);
            Console.WriteLine(movies[79]);

            movies.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            Console.WriteLine("Starting importing movies...");

            foreach (var movie in movies)
            {
                db.Movies?.Add(movie);
                await db.SaveChangesAsync();
            }

            Console.WriteLine("Finished importing movies...");
        }

        private static async Task ImportConcerts(AppDbContext db)
        {
            List<Concert> concerts = new();
            int count = 0;
            string fileName = "Data/col_music.bson";
            using var stream = File.OpenRead(fileName);
            using var reader = new BsonBinaryReader(stream);

            while (!reader.IsAtEndOfFile())
            {
                Concert concert = new();

                reader.ReadStartDocument();
                string _i = reader.ReadName();
                ObjectId _idValue = reader.ReadObjectId();

                string _t = reader.ReadName();
                string artistValue = reader.ReadString();
                concert.Artist = artistValue;

                string _d = reader.ReadName();
                if (reader.CurrentBsonType is BsonType.String)
                {
                    string dateValue = reader.ReadString();
                    DateTime date;
                    if (DateTime.TryParse(dateValue, out date))
                    {
                        concert.SeenAt = date;
                    }
                }
                else
                {
                    long dateValue = reader.ReadDateTime();
                    var date = DateTimeOffset.FromUnixTimeMilliseconds(dateValue);
                    concert.SeenAt = date.DateTime;
                }

                string _im = reader.ReadName();
                string locationValue = reader.ReadString();
                concert.Location = locationValue;

                string _g = reader.ReadName();
                reader.ReadStartDocument();
                string _gi = reader.ReadName();
                string genreIdentifierValue = reader.ReadString();
                concert.Genre = ToConcertGenre(genreIdentifierValue);
                string _x = reader.ReadName();
                string _c = reader.ReadString();
                reader.ReadEndDocument();

                string _b = reader.ReadName();
                string ratingValue = reader.ReadString();
                concert.Rating = ToRating(ratingValue);

                string _k = reader.ReadName();
                ObjectId _l = reader.ReadObjectId();

                string _s = reader.ReadName();
                long createdValue = reader.ReadDateTime();
                var createdDate = DateTimeOffset.FromUnixTimeMilliseconds(createdValue);
                concert.CreatedAt = createdDate.UtcDateTime;

                string _y = reader.ReadName();
                long updatedValue = reader.ReadDateTime();
                var updatedDate = DateTimeOffset.FromUnixTimeMilliseconds(updatedValue);
                concert.UpdatedAt = updatedDate.UtcDateTime;

                string _q = reader.ReadName();
                if (reader.CurrentBsonType is BsonType.DateTime)
                {
                    long deletedValue = reader.ReadDateTime();
                    DateTimeOffset deletedDate = DateTimeOffset.FromUnixTimeMilliseconds(deletedValue);
                    concert.DeletedAt = deletedDate.UtcDateTime;
                }
                else
                {
                    reader.SkipValue();
                }

                reader.ReadEndDocument();

                concert.AccountId = 1;

                concerts.Add(concert);

                count++;
            }

            Console.WriteLine(count);
            Console.WriteLine(concerts.Count);

            reader.Close();
            stream.Close();

            Console.WriteLine(concerts[0]);

            concerts.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            Console.WriteLine("Starting importing movies...");

            foreach (var concert in concerts)
            {
                db.Concerts?.Add(concert);
                await db.SaveChangesAsync();
            }

            Console.WriteLine("Finished importing movies...");
        }

        private static async Task ImportTheaters(AppDbContext db)
        {
            List<Theater> theaters = new();
            int count = 0;
            string fileName = "Data/col_operas.bson";
            using var stream = File.OpenRead(fileName);
            using var reader = new BsonBinaryReader(stream);

            while (!reader.IsAtEndOfFile())
            {
                Theater theater = new();

                reader.ReadStartDocument();
                string _i = reader.ReadName();
                ObjectId _idValue = reader.ReadObjectId();

                string _t = reader.ReadName();
                string titleValue = reader.ReadString();
                theater.Title = titleValue;

                string _d = reader.ReadName();
                if (reader.CurrentBsonType is BsonType.String)
                {
                    string dateValue = reader.ReadString();
                    DateTime date;
                    if (DateTime.TryParse(dateValue, out date))
                    {
                        theater.SeenAt = date;
                    }
                }
                else
                {
                    long dateValue = reader.ReadDateTime();
                    var date = DateTimeOffset.FromUnixTimeMilliseconds(dateValue);
                    theater.SeenAt = date.DateTime;
                }

                string _im = reader.ReadName();
                string locationValue = reader.ReadString();
                theater.Location = locationValue;

                string _g = reader.ReadName();
                reader.ReadStartDocument();
                string _gi = reader.ReadName();
                string genreIdentifierValue = reader.ReadString();
                theater.Genre = ToTheaterGenre(genreIdentifierValue);
                string _x = reader.ReadName();
                string _c = reader.ReadString();
                reader.ReadEndDocument();

                string _b = reader.ReadName();
                string ratingValue = reader.ReadString();
                theater.Rating = ToRating(ratingValue);

                string _k = reader.ReadName();
                ObjectId _l = reader.ReadObjectId();

                string _s = reader.ReadName();
                long createdValue = reader.ReadDateTime();
                var createdDate = DateTimeOffset.FromUnixTimeMilliseconds(createdValue);
                theater.CreatedAt = createdDate.UtcDateTime;

                string _y = reader.ReadName();
                long updatedValue = reader.ReadDateTime();
                var updatedDate = DateTimeOffset.FromUnixTimeMilliseconds(updatedValue);
                theater.UpdatedAt = updatedDate.UtcDateTime;

                string _q = reader.ReadName();
                if (reader.CurrentBsonType is BsonType.DateTime)
                {
                    long deletedValue = reader.ReadDateTime();
                    DateTimeOffset deletedDate = DateTimeOffset.FromUnixTimeMilliseconds(deletedValue);
                    theater.DeletedAt = deletedDate.UtcDateTime;
                }
                else
                {
                    reader.SkipValue();
                }

                reader.ReadEndDocument();

                theater.AccountId = 1;

                theaters.Add(theater);

                count++;
            }

            Console.WriteLine(count);
            Console.WriteLine(theaters.Count);

            reader.Close();
            stream.Close();

            theaters.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));

            Console.WriteLine("Starting importing movies...");

            foreach (var theater in theaters)
            {
                db.Theaters?.Add(theater);
                await db.SaveChangesAsync();
            }

            Console.WriteLine("Finished importing movies...");
        }

        private static async Task GetPosterUrls(AppDbContext db)
        {
            var movies = await db.Movies.ToListAsync();
            var endpoint = $"http://www.omdbapi.com/";
            var client = new HttpClient();

            foreach (var movie in movies)
            {
                var uri = $"http://www.omdbapi.com/?i={movie.ImdbIdentifier}&apiKey=";

                var res = await client.GetFromJsonAsync<Result>(uri);

                if (res is not null)
                {
                    Console.WriteLine($"{movie.ImdbIdentifier}: {res.Poster}");

                    movie.PosterUrl = res.Poster;
                    db.Movies.Update(movie);
                    await db.SaveChangesAsync();
                }
            }
        }

        public class Result
        {
            public string Poster { get; set; }
        }

        private static MovieGenre ToMovieGenre(string genre) => genre switch
        {
            "action" => MovieGenre.Action,
            "comedy" => MovieGenre.Comedy,
            "drama" => MovieGenre.Drama,
            "fantasy" => MovieGenre.Fantasy,
            "horror" => MovieGenre.Horror,
            "sifi" => MovieGenre.SciFi,
            "thriller" => MovieGenre.Thriller,
            _ => MovieGenre.Action
        };

        private static ConcertGenre ToConcertGenre(string genre) => genre switch
        {
            "rock" => ConcertGenre.Rock,
            "classic" => ConcertGenre.Classic,
            "reggae" => ConcertGenre.Reggae,
            "pop" => ConcertGenre.Pop,
            "latin" => ConcertGenre.Latin,
            "electro" => ConcertGenre.Electro,
            "drumnbass" => ConcertGenre.DrumnBass,
            _ => ConcertGenre.Rock
        };

        private static TheaterGenre ToTheaterGenre(string genre) => genre switch
        {
            "operette" => TheaterGenre.Operette,
            "opera" => TheaterGenre.Opera,
            "musical" => TheaterGenre.Musical,
            "theater" => TheaterGenre.Theater,
            "ballet" => TheaterGenre.Ballet,
            _ => TheaterGenre.Theater
        };

        private static Rating ToRating(string rating) => rating switch
        {
            "1" => Rating.Bad,
            "2" => Rating.NotGood,
            "3" => Rating.Ok,
            "4" => Rating.Good,
            "5" => Rating.VeryGood,
            _ => Rating.Ok
        };
    }

    public class AppDbContext : DbContext
    {
        // public DbSet<Account>? Accounts { get; set; }
        public DbSet<Movie>? Movies { get; set; }
        public DbSet<Theater>? Theaters { get; set; }
        public DbSet<Concert>? Concerts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=..\\MovieDB.Api\\Data\\moviedb.db");
        }
    }
}
