using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;

public record TracksResponseDto
{
    [JsonProperty("data")] public TracksResponseItemDto Data { get; set; }

    public static readonly Func<MediaContext, Guid, IAsyncEnumerable<TrackUser>> GetTracks =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId) => mediaContext.TrackUser
            .AsNoTracking()
            .Where(u => u.UserId == userId)
            .Include(trackUser => trackUser.Track)
            .ThenInclude(track => track.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Album)
            .Include(trackUser => trackUser.Track)
            .ThenInclude(track => track.ArtistTrack)
            .ThenInclude(artistTrack => artistTrack.Artist));
}