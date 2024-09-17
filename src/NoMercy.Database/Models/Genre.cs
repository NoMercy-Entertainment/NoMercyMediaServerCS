#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Genre
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    public ICollection<GenreMovie> GenreMovies { get; set; }
    public ICollection<GenreTv> GenreTvShows { get; set; }

    public Genre()
    {
    }

    // public Genre(Providers.TMDB.Models.Shared.TmdbGenre tmdbGenre)
    // {
    //     Id = tmdbGenre.Id;
    //     Name = tmdbGenre.Name;
    // }
}