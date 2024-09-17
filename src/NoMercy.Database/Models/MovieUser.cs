#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(MovieId), nameof(UserId))]
[Index(nameof(MovieId))]
[Index(nameof(UserId))]
public class MovieUser
{
    [JsonProperty("movie_id")] public int MovieId { get; set; }
    public Movie Movie { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public MovieUser()
    {
    }

    public MovieUser(int movieId, Guid userId)
    {
        MovieId = movieId;
        UserId = userId;
    }
}