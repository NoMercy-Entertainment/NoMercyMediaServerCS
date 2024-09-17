using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;

public record PlaylistResponseDto
{
    [JsonProperty("data")] public List<PlaylistResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, IAsyncEnumerable<Playlist>> GetPlaylists =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId) => mediaContext.Playlists
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .Include(playlist => playlist.Tracks)
        );

    public static readonly Func<MediaContext, Guid, Ulid, Task<Playlist?>> GetPlaylist =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id) => mediaContext.Playlists
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Where(u => u.UserId == userId)
            .Include(playlist => playlist.Tracks)
            .ThenInclude(trackUser => trackUser.Track)
            .ThenInclude(track => track.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Album)
            .Include(playlist => playlist.Tracks)
            .ThenInclude(trackUser => trackUser.Track)
            .ThenInclude(track => track.ArtistTrack)
            .ThenInclude(artistTrack => artistTrack.Artist)
            .FirstOrDefault()
        );
}

public record PlaylistResponseItemDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("tracks")] public ICollection<PlaylistTrack> Tracks { get; set; }

    public PlaylistResponseItemDto(Playlist playlist)
    {
        Id = playlist.Id;
        Name = playlist.Name;
        Description = playlist.Description;
        Cover = playlist.Cover;
        ColorPalette = playlist.ColorPalette;
        CreatedAt = playlist.CreatedAt;
        UpdatedAt = playlist.UpdatedAt;
        Tracks = playlist.Tracks;
        Type = "playlists";
    }
}