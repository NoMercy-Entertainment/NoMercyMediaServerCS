using NoMercy.Database.Models;

namespace NoMercy.MediaProcessing.MusicGenres;

public interface IMusicGenreRepository
{
    public Task Store(MusicGenre musicGenre);
    public Task LinkToArtist(ArtistMusicGenre genreArtist);
    public Task LinkToRecording(MusicGenreTrack genreRecording);
    public Task LinkToReleaseGroup(MusicGenreReleaseGroup genreReleaseGroup);
    public Task LinkToRelease(AlbumMusicGenre genreRelease);
}