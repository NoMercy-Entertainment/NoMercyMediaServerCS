#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(SpecialId), nameof(UserId))]
public class SpecialUser
{
    [JsonProperty("special_id")] public Ulid SpecialId { get; set; }
    public Special Special { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public SpecialUser()
    {
    }

    public SpecialUser(Ulid specialId, Guid userId)
    {
        SpecialId = specialId;
        UserId = userId;
    }
}