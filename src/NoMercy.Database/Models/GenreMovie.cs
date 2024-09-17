#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(GenreId), nameof(MovieId))]
[Index(nameof(GenreId))]
[Index(nameof(MovieId))]
public class GenreMovie
{
    [JsonProperty("genre_id")] public int GenreId { get; set; }
    public Genre Genre { get; set; }

    [JsonProperty("movie_id")] public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public GenreMovie()
    {
    }
}