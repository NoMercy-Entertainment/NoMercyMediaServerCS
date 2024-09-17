#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(LibraryId), nameof(UserId))]
[Index(nameof(UserId))]
[Index(nameof(LibraryId))]
public class LibraryUser
{
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    public Library Library { get; set; }

    [JsonProperty("user_id")] public Guid UserId { get; set; }
    public User User { get; set; }

    public LibraryUser()
    {
    }

    public LibraryUser(Ulid libraryId, Guid userId)
    {
        LibraryId = libraryId;
        UserId = userId;
    }
}