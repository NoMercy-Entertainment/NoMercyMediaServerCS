#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(TrackId), nameof(UserId))]
[Index(nameof(TrackId))]
[Index(nameof(UserId))]
public class TrackUser
{
    [JsonProperty("track_id")] public Guid TrackId { get; set; }
    public Track Track { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public TrackUser()
    {
    }

    public TrackUser(Guid trackId, Guid userId)
    {
        TrackId = trackId;
        UserId = userId;
    }
}