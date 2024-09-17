#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(UserId))]
[Index(nameof(TrackId))]
public class MusicPlay : Timestamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    [JsonProperty("track_id")] public Guid TrackId { get; set; }
    public Track Track { get; set; }

    public MusicPlay()
    {
    }

    public MusicPlay(Guid userId, Guid trackId)
    {
        UserId = userId;
        TrackId = trackId;
    }
}