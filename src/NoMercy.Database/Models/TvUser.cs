#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(TvId), nameof(UserId))]
[Index(nameof(TvId))]
[Index(nameof(UserId))]
public class TvUser
{
    [JsonProperty("tv_id")] public int TvId { get; set; }
    public Tv Tv { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public TvUser()
    {
    }

    public TvUser(int tvId, Guid userId)
    {
        TvId = tvId;
        UserId = userId;
    }
}