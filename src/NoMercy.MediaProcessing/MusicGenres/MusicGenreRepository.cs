using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.MusicGenres;

public class MusicGenreRepository(MediaContext context) : IMusicGenreRepository
{
    public Task Store(MusicGenre genre)
    {
        return context.MusicGenres.Upsert(genre)
            .On(v => new { v.Id })
            .WhenMatched(v => new MusicGenre
            {
                Id = v.Id,
                Name = v.Name
            })
            .RunAsync();
    }

    public Task LinkToReleaseGroup(MusicGenreReleaseGroup genreReleaseGroup)
    {
        return context.MusicGenreReleaseGroup.Upsert(genreReleaseGroup)
            .On(e => new { e.GenreId, e.ReleaseGroupId })
            .WhenMatched((s, i) => new MusicGenreReleaseGroup
            {
                GenreId = i.GenreId,
                ReleaseGroupId = i.ReleaseGroupId
            })
            .RunAsync();
    }

    public Task LinkToArtist(ArtistMusicGenre genreArtist)
    {
        return context.ArtistMusicGenre.Upsert(genreArtist)
            .On(e => new { e.MusicGenreId, e.ArtistId })
            .WhenMatched((s, i) => new ArtistMusicGenre
            {
                MusicGenreId = i.MusicGenreId,
                ArtistId = i.ArtistId
            })
            .RunAsync();
    }

    public Task LinkToRelease(AlbumMusicGenre genreRelease)
    {
        return context.AlbumMusicGenre.Upsert(genreRelease)
            .On(e => new { e.MusicGenreId, e.AlbumId })
            .WhenMatched((s, i) => new AlbumMusicGenre
            {
                MusicGenreId = i.MusicGenreId,
                AlbumId = i.AlbumId
            })
            .RunAsync();
    }

    public Task LinkToRecording(MusicGenreTrack genreRecording)
    {
        return context.MusicGenreTrack.Upsert(genreRecording)
            .On(e => new { e.GenreId, e.TrackId })
            .WhenMatched((s, i) => new MusicGenreTrack
            {
                GenreId = i.GenreId,
                TrackId = i.TrackId
            })
            .RunAsync();
    }
}