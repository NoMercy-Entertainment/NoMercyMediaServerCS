#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(ArtistId), nameof(UserId))]
[Index(nameof(ArtistId))]
[Index(nameof(UserId))]
public class ArtistUser
{
    [JsonProperty("artist_id")] public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public ArtistUser()
    {
    }

    public ArtistUser(Guid artistId, Guid userId)
    {
        ArtistId = artistId;
        UserId = userId;
    }
}