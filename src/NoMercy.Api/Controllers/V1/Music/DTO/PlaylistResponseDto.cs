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