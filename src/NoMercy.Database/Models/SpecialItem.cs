#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(SpecialId), nameof(EpisodeId), IsUnique = true)]
[Index(nameof(SpecialId), nameof(MovieId), IsUnique = true)]
public class SpecialItem
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("order")] public int Order { get; set; }

    [JsonProperty("special_id")] public Ulid SpecialId { get; set; }
    [JsonProperty("special")] public Special Special { get; set; }

    [JsonProperty("episode_id")] public int? EpisodeId { get; set; }
    public Episode? Episode { get; set; }

    [JsonProperty("movie_id")] public int? MovieId { get; set; }
    public Movie? Movie { get; set; }

    [JsonProperty("user_data")] public ICollection<UserData> UserData { get; set; }
}