#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(CollectionId), nameof(UserId))]
[Index(nameof(CollectionId))]
[Index(nameof(UserId))]
public class CollectionUser
{
    [JsonProperty("collection_id")] public int CollectionId { get; set; }
    public Collection Collection { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public CollectionUser()
    {
    }

    public CollectionUser(int collectionId, Guid userId)
    {
        CollectionId = collectionId;
        UserId = userId;
    }
}