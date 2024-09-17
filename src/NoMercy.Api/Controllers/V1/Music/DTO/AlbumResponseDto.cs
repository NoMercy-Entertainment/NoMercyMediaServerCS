using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.Api.Controllers.V1.Music.DTO;

public record AlbumResponseDto
{
    [JsonProperty("data")] public AlbumResponseItemDto? Data { get; set; }

    public static readonly Func<MediaContext, Guid, Guid, Task<Album?>> GetAlbum =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Guid id) => mediaContext.Albums
            .AsNoTracking()
            .Where(album => album.Id == id)
            .Where(album => album.Library.LibraryUsers
                .FirstOrDefault(libraryUser => libraryUser.UserId == userId) != null)
            .Include(album => album.Library)
            .Include(album => album.AlbumUser
                .Where(albumUser => albumUser.UserId == userId)
            )
            .Include(album => album.AlbumArtist)
            .ThenInclude(albumArtist => albumArtist.Artist)
            .ThenInclude(artist => artist.Translations)
            .Include(album => album.AlbumTrack
                .OrderBy(albumTrack => albumTrack.Track.DiscNumber)
                .ThenBy(albumTrack => albumTrack.Track.TrackNumber)
                .Where(artistTrack => artistTrack.Track.Duration != null)
            )
            .Include(album => album.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Track)
            .ThenInclude(track => track.TrackUser
                .Where(albumUser => albumUser.UserId == userId)
            )
            .Include(album => album.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Track)
            .ThenInclude(track => track.ArtistTrack)
            .ThenInclude(artistTrack => artistTrack.Artist)
            .ThenInclude(artist => artist.Translations)
            .Include(album => album.AlbumArtist)
            .ThenInclude(albumArtist => albumArtist.Artist)
            .ThenInclude(artist => artist.Images)
            .Include(album => album.Images)
            .Include(album => album.Translations)
            .Include(album => album.AlbumMusicGenre)
            .ThenInclude(artistMusicGenre => artistMusicGenre.MusicGenre)
            .FirstOrDefault());
}