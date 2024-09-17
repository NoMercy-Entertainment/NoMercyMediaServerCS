using Newtonsoft.Json;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Dashboard.DTO;

public class PermissionsResponseItemDto : User
{
    [JsonProperty("library_user")] public new LibraryUserDto[] LibraryUser { get; set; }
    [JsonProperty("libraries")] public Ulid[] Libraries { get; set; }

    public PermissionsResponseItemDto(User user)
    {
        Id = user.Id;
        Email = user.Email;
        Manage = user.Manage;
        Owner = user.Owner;
        Name = user.Name;
        Allowed = user.Allowed;
        AudioTranscoding = user.AudioTranscoding;
        VideoTranscoding = user.VideoTranscoding;
        NoTranscoding = user.NoTranscoding;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;

        LibraryUser = user.LibraryUser
            .Select(libraryUser => new LibraryUserDto
            {
                LibraryId = libraryUser.LibraryId,
                UserId = libraryUser.UserId
            })
            .ToArray();

        Libraries = user.LibraryUser
            .Select(libraryUser => libraryUser.Library.Id)
            .ToArray();
    }
}

public class LibraryUserDto
{
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("UserId")] public Guid UserId { get; set; }
}

public class UserRequest
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
    [JsonProperty("manage")] public bool Manage { get; set; }
    [JsonProperty("owner")] public bool Owner { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("allowed")] public bool Allowed { get; set; }
    [JsonProperty("audio_transcoding")] public bool AudioTranscoding { get; set; }
    [JsonProperty("video_transcoding")] public bool VideoTranscoding { get; set; }
    [JsonProperty("no_transcoding")] public bool NoTranscoding { get; set; }
}

public class UserPermissionRequest
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("manage")] public bool Manage { get; set; }
    [JsonProperty("owner")] public bool Owner { get; set; }
    [JsonProperty("allowed")] public bool Allowed { get; set; }
    [JsonProperty("audio_transcoding")] public bool AudioTranscoding { get; set; }
    [JsonProperty("video_transcoding")] public bool VideoTranscoding { get; set; }
    [JsonProperty("no_transcoding")] public bool NoTranscoding { get; set; }
    [JsonProperty("libraries")] public Ulid[] Libraries { get; set; }

    public UserPermissionRequest()
    {
        //
    }

    public UserPermissionRequest(User user)
    {
        Id = user.Id;
        Manage = user.Manage;
        Owner = user.Owner;
        Allowed = user.Allowed;
        AudioTranscoding = user.AudioTranscoding;
        VideoTranscoding = user.VideoTranscoding;
        NoTranscoding = user.NoTranscoding;

        Libraries = user.LibraryUser
            .Select(libraryUser => libraryUser.LibraryId)
            .ToArray();
    }
}