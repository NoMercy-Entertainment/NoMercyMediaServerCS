#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(AlbumId), nameof(UserId))]
[Index(nameof(AlbumId))]
[Index(nameof(UserId))]
public class AlbumUser
{
    [JsonProperty("album_id")] public Guid AlbumId { get; set; }
    public Album Album { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public AlbumUser()
    {
    }

    public AlbumUser(Guid albumId, Guid userId)
    {
        AlbumId = albumId;
        UserId = userId;
    }
}