using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Api.Controllers.V1.Media.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Music.DTO;

public record ArtistResponseDto
{
    [JsonProperty("data")] public ArtistResponseItemDto? Data { get; set; }

    public static readonly Func<MediaContext, Guid, Guid, Task<Artist?>> GetArtist =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Guid id) =>
            mediaContext.Artists
                .AsNoTracking()
                .Where(album => album.Id == id)
                .Where(artist => artist.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
                
                .Include(artist => artist.Library)
                
                .Include(artist => artist.ArtistUser
                    .Where(artistUser => artistUser.UserId == userId)
                )
                .ThenInclude(artistUser => artistUser.User)
                .ThenInclude(user => user.TrackUser)
                .ThenInclude(trackUser => trackUser.Track)
                
                .Include(artist => artist.AlbumArtist)
                .ThenInclude(albumArtist => albumArtist.Album)
                .ThenInclude(albumArtist => albumArtist.AlbumUser)
                
                .Include(artist => artist.AlbumArtist)
                .ThenInclude(albumArtist => albumArtist.Album)
                .ThenInclude(artist => artist.Translations)
                
                .Include(artist => artist.ArtistTrack
                    .OrderBy(artistTrack => artistTrack.Track.TrackNumber)
                    .ThenBy(artistTrack => artistTrack.Track.DiscNumber)
                    .Where(artistTrack => artistTrack.Track.Duration != null)
                )
                .ThenInclude(artistTrack => artistTrack.Track)
                .ThenInclude(track => track.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Album)
                .ThenInclude(album => album.AlbumArtist)
                
                .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
                .ThenInclude(track => track.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Album)
                .ThenInclude(album => album.AlbumUser
                    .Where(albumUser => albumUser.UserId == userId)
                )
                
                .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
                .ThenInclude(track => track.AlbumTrack)
                .ThenInclude(albumTrack => albumTrack.Album)
                .ThenInclude(artist => artist.Translations)
                
                .Include(artist => artist.ArtistTrack)
                .ThenInclude(artistTrack => artistTrack.Track)
                .ThenInclude(track => track.TrackUser
                    .Where(trackUser => trackUser.UserId == userId)
                )
                .ThenInclude(trackUser => trackUser.User)
                
                .Include(artist => artist.ArtistReleaseGroup)
                .ThenInclude(artistReleaseGroup => artistReleaseGroup.ReleaseGroup)
                .ThenInclude(releaseGroup => releaseGroup.AlbumReleaseGroup)
                .ThenInclude(albumReleaseGroup => albumReleaseGroup.Album)
                .ThenInclude(artist => artist.Translations)
                
                .Include(artist => artist.ArtistReleaseGroup)
                .ThenInclude(artistReleaseGroup => artistReleaseGroup.ReleaseGroup)
                .ThenInclude(releaseGroup => releaseGroup.AlbumReleaseGroup)
                .ThenInclude(albumReleaseGroup => albumReleaseGroup.Album)
                .ThenInclude(album => album.AlbumArtist)
                
                .Include(artist => artist.Images)
                
                .Include(artist => artist.Translations)
                
                .Include(artist => artist.ArtistMusicGenre)
                .ThenInclude(artistMusicGenre => artistMusicGenre.MusicGenre)
                
                .FirstOrDefault());
}

public record ArtistResponseItemDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("country")] public string? Country { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }

    [JsonProperty("playlists")] public IEnumerable<AlbumDto> Playlists { get; set; }
    [JsonProperty("tracks")] public IEnumerable<ArtistTrackDto> Tracks { get; set; }
    [JsonProperty("favorite_tracks")] public IEnumerable<ArtistTrackDto> FavoriteTracks { get; set; }
    [JsonProperty("images")] public IEnumerable<ImageDto> Images { get; set; }
    [JsonProperty("genres")] public IEnumerable<GenreDto> Genres { get; set; }
    [JsonProperty("albums")] public IEnumerable<AlbumDto> Albums { get; set; }
    [JsonProperty("featured")] public IEnumerable<AlbumDto> Featured { get; set; }

    public ArtistResponseItemDto(Artist artist, Guid userId, string? country = "US")
    {
        ColorPalette = artist.ColorPalette;
        Cover = artist.Cover;
        Disambiguation = artist.Disambiguation;
        Description = artist.Description;
        Favorite = artist.ArtistUser.Any();
        Folder = artist.Folder;
        Id = artist.Id;
        LibraryId = artist.LibraryId;
        Name = artist.Name;
        Type = "artists";

        Genres = artist.ArtistMusicGenre
            .Select(artistMusicGenre => new GenreDto(artistMusicGenre));

        Images = artist.Images.Select(image => new ImageDto(image));
        
        Albums = artist.AlbumArtist
            .Select(album => new AlbumDto(album, country!))
            .GroupBy(album => album.Id)
            .Select(album => album.First())
            .OrderBy(artistTrack => artistTrack.Year);
        
        Featured = artist.ArtistTrack
            .Select(artistTrack => artistTrack.Track.AlbumTrack.First().Album)
            .GroupBy(album => album.Name.RemoveNonAlphaNumericCharacters())
            .Select(album => album.First())
            .OrderBy(album => album.Year)
            .Where(album => Albums.All(albumDto => albumDto.Id != album.Id))
            .Select(album => new AlbumDto(album, country!))
            .OrderBy(artistTrack => artistTrack.Year)
            .ToList();
        
        Playlists = artist.AlbumArtist
            .DistinctBy(albumArtist => albumArtist.AlbumId)
            .Where(album => album.Album.AlbumUser.Any(user => user.UserId == userId))
            .Select(trackAlbum => new AlbumDto(trackAlbum, country!))
            .OrderBy(album => album.Year);

        Tracks = artist.ArtistTrack
            .Select(artistTrack => new ArtistTrackDto(artistTrack, country!))
            .GroupBy(artistTrack => artistTrack.AlbumName + artistTrack.Name)
            .Select(artistTrack => artistTrack.First())
            .OrderBy(artistTrack => artistTrack.AlbumName)
            .ThenBy(artistTrack => artistTrack.Disc)
            .ThenBy(artistTrack => artistTrack.Track);

        // :TODO this speaks for itself....
        MediaContext context = new();
        List<ArtistTrackDto> favorites = context.MusicPlays
            .AsNoTracking()
            .Where(musicPlay => musicPlay.UserId == userId)
            .Where(musicPlay => musicPlay.Track.ArtistTrack
                .Any(artistTrack => artistTrack.ArtistId == artist.Id))
            
            .Include(musicPlay => musicPlay.Track)
            .ThenInclude(track => track.TrackUser)
            
            .Include(musicPlay => musicPlay.Track)
            .ThenInclude(track => track.AlbumTrack)
            .ThenInclude(albumTrack => albumTrack.Album)
            .ThenInclude(albumTrack => albumTrack.Translations)
            
            .Include(musicPlay => musicPlay.Track)
            .ThenInclude(track => track.ArtistTrack)
            .ThenInclude(albumTrack => albumTrack.Artist)
            .ThenInclude(album => album.Translations)
            .Select(artistTrack => new ArtistTrackDto(artistTrack.Track, country!))
            .ToList();

        FavoriteTracks = favorites.GroupBy(musicPlay => musicPlay.Id)
            .OrderByDescending(musicPlay => musicPlay.Count())
            .Select(musicPlay => musicPlay.First());
    }
}

public record ArtistTrackDto
{
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("date")] public DateTime? Date { get; set; }
    [JsonProperty("disc")] public int? Disc { get; set; }
    [JsonProperty("duration")] public string? Duration { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string? Filename { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("folder_id")] public Ulid? FolderId { get; set; }
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("path")] public string Path { get; set; }
    [JsonProperty("quality")] public int? Quality { get; set; }
    [JsonProperty("track")] public int? Track { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }
    [JsonProperty("album_id")] public Guid AlbumId { get; set; }
    [JsonProperty("album_name")] public string AlbumName { get; set; }

    [JsonProperty("album_track")] public IEnumerable<AlbumDto> Album { get; set; }
    [JsonProperty("artist_track")] public IEnumerable<ArtistDto> Artist { get; set; }

    public ArtistTrackDto(ArtistTrack artistTrack, string country)
    {
        ColorPalette = artistTrack.Track.AlbumTrack.FirstOrDefault()?.Album.ColorPalette;
        Cover = artistTrack.Track.AlbumTrack.FirstOrDefault()?.Album.Cover;
        Date = artistTrack.Track.Date;
        Disc = artistTrack.Track.DiscNumber;
        AlbumId = artistTrack.Track.AlbumTrack.First().AlbumId;
        AlbumName = artistTrack.Track.AlbumTrack.First().Album.Name;
        Duration = artistTrack.Track.Duration;
        Favorite = artistTrack.Track.TrackUser.Any();
        Filename = artistTrack.Track.Filename;
        Folder = artistTrack.Track.Folder;
        FolderId = artistTrack.Track.FolderId;
        Id = artistTrack.Track.Id;
        LibraryId = artistTrack.Artist.LibraryId;
        Name = artistTrack.Track.Name;
        Origin = Info.DeviceId;
        Path = artistTrack.Track.Folder + "/" + artistTrack.Track.Filename;
        Quality = artistTrack.Track.Quality;
        Track = artistTrack.Track.TrackNumber;
        Lyrics = artistTrack.Track.Lyrics;
        Type = "tracks";

        Album = artistTrack.Track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack, country));

        Artist = artistTrack.Track.ArtistTrack
            .Select(albumTrack => new ArtistDto(albumTrack, country));
    }


    public ArtistTrackDto(Track track, string? country = "US")
    {
        ColorPalette = track.AlbumTrack.First().Album.ColorPalette ?? track.ArtistTrack.First().Artist.ColorPalette;
        Cover = track.AlbumTrack.First().Album.Cover ?? track.ArtistTrack.First().Artist.Cover;
        Date = track.UpdatedAt;
        Disc = track.DiscNumber;
        Duration = track.Duration;
        Favorite = track.TrackUser.Any();
        Filename = track.Filename;
        Folder = track.Folder;
        FolderId = track.FolderId;
        Id = track.Id;
        LibraryId = track.AlbumTrack.First().Album.LibraryId;
        Name = track.Name;
        Origin = Info.DeviceId;
        Path = track.Folder + "/" + track.Filename;
        Quality = track.Quality;
        Track = track.TrackNumber;
        Type = "tracks";
        AlbumName = track.AlbumTrack.First().Album.Name;

        Album = track.AlbumTrack
            .DistinctBy(trackAlbum => trackAlbum.AlbumId)
            .Select(albumTrack => new AlbumDto(albumTrack, country!));

        Artist = track.ArtistTrack
            .DistinctBy(trackArtist => trackArtist.ArtistId)
            .Select(trackArtist => new ArtistDto(trackArtist, country!));
    }
}

public record ArtistDto
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("disambiguation")] public string? Disambiguation { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("folder")] public string? Folder { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("type")] public string Type { get; set; }

    public ArtistDto(AlbumArtist albumArtist, string country)
    {
        string? description = albumArtist.Artist.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : albumArtist.Album.Description;

        Id = albumArtist.Artist.Id;
        Name = albumArtist.Artist.Name;
        Disambiguation = albumArtist.Artist.Disambiguation;
        Cover = albumArtist.Artist.Cover;
        Folder = albumArtist.Artist.Folder;
        ColorPalette = albumArtist.Artist.ColorPalette;
        LibraryId = albumArtist.Artist.LibraryId;
        Origin = Info.DeviceId;
        Type = "artists";
    }

    public ArtistDto(ArtistTrack artistTrack, string country)
    {
        // string? description = artistTrack.Artist.Translations
        //     .FirstOrDefault(translation => translation.Iso31661 == country)?
        //     .Description;

        // Description = !string.IsNullOrEmpty(description)
        //     ? description
        //     : artistTrack.Artist.Description;
        Description = artistTrack.Artist.Description;

        Id = artistTrack.Artist.Id;
        Name = artistTrack.Artist.Name;
        Disambiguation = artistTrack.Artist.Disambiguation;
        Cover = artistTrack.Artist.Cover;
        Folder = artistTrack.Artist.Folder;
        ColorPalette = artistTrack.Artist.ColorPalette;
        LibraryId = artistTrack.Artist.LibraryId;
        Origin = Info.DeviceId;
        Type = "artists";
    }
}

public record ReleaseGroupDto
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("cover")] public string? Cover { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("library_id")] public Ulid? LibraryId { get; set; }
    [JsonProperty("origin")] public Guid Origin { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int Year { get; set; }

    public ReleaseGroupDto(AlbumReleaseGroup artistReleaseGroup, string country)
    {
        string? description = artistReleaseGroup.ReleaseGroup.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?
            .Description;

        Description = !string.IsNullOrEmpty(description)
            ? description
            : artistReleaseGroup.ReleaseGroup.Description;

        Id = artistReleaseGroup.ReleaseGroupId;
        Title = artistReleaseGroup.ReleaseGroup.Title;
        Cover = artistReleaseGroup.ReleaseGroup.Cover;
        ColorPalette = artistReleaseGroup.ReleaseGroup.ColorPalette;
        LibraryId = artistReleaseGroup.ReleaseGroup.LibraryId;
        Origin = Info.DeviceId;
        Type = "release_groups";
        Year = artistReleaseGroup.ReleaseGroup.Year;
    }
}