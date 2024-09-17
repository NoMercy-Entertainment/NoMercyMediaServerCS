using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record LibraryResponseDto
{
    [JsonProperty("cursor")] public long? Cursor { get; set; }

    [JsonProperty("data")] public List<LibraryResponseItemDto> Data { get; set; }

    public static async Task<List<Movie>> GetLibraryMovies(Guid userId, Ulid libraryId, string language, int take,
        int page = 0)
    {
        await using MediaContext mediaContext = new();

        IIncludableQueryable<Movie, Certification> query = mediaContext.Movies
                .AsNoTracking()
                .Where(movie => movie.Library.Id == libraryId)
                .Where(movie => movie.Library.LibraryUsers.Any(u => u.UserId == userId))
                .Where(libraryMovie => libraryMovie.VideoFiles
                    .Any(videoFile => videoFile.Folder != null) == true
                )
                .Include(movie => movie.VideoFiles)
                .Include(movie => movie.Media
                    .Where(media => media.Iso6391 == language || media.Iso6391 == "en")
                )
                .Include(movie => movie.Images
                    .Where(image => image.Iso6391 == language || image.Iso6391 == "en")
                )
                .Include(movie => movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre)
                .Include(movie => movie.Translations
                    .Where(translation => translation.Iso6391 == language || translation.Iso6391 == "en")
                )
                .Include(movie => movie.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
            ;

        List<Movie> movies = await query
            .OrderBy(collection => collection.TitleSort)
            .Skip(page * take)
            .Take(take)
            .ToListAsync();

        return movies;
    }

    public static async Task<List<Tv>> GetLibraryShows(Guid userId, Ulid libraryId, string language, int take,
        int page = 0)
    {
        await using MediaContext mediaContext = new();

        IIncludableQueryable<Tv, Certification> query = mediaContext.Tvs.AsNoTracking()
                .Where(tv => tv.Library.Id == libraryId)
                .Where(tv => tv.Library.LibraryUsers.Any(u => u.UserId == userId))
                .Where(libraryTv => libraryTv.Episodes
                    .Any(episode => episode.VideoFiles
                        .Any(videoFile => videoFile.Folder != null) == true
                    ) == true)
                .Include(tv => tv.Episodes
                    .Where(episode => episode.SeasonNumber > 0 && episode.VideoFiles.Count != 0)
                )
                .ThenInclude(episode => episode.VideoFiles)
                .Include(tv => tv.Media
                    .Where(media => media.Iso6391 == language || media.Iso6391 == "en")
                )
                .Include(tv => tv.Images
                    .Where(image => image.Iso6391 == language || image.Iso6391 == "en")
                )
                .Include(tv => tv.GenreTvs)
                .ThenInclude(genreTv => genreTv.Genre)
                .Include(tv => tv.Translations
                    .Where(translation => translation.Iso6391 == language || translation.Iso6391 == "en")
                )
                .Include(tv => tv.CertificationTvs)
                .ThenInclude(certificationTv => certificationTv.Certification)
            ;

        List<Tv> shows = await query
            .OrderBy(collection => collection.TitleSort)
            .Skip(page * take)
            .Take(take)
            .ToListAsync();

        return shows;
    }

    public static readonly Func<MediaContext, Guid, Ulid, string, int, int, Task<Library>> GetLibrary =
        EF.CompileAsyncQuery(
            (MediaContext mediaContext, Guid userId, Ulid id, string language, int take, int page = 0) =>
                mediaContext.Libraries.AsNoTracking()
                    .Where(library => library.Id == id)
                    .Where(library => library.LibraryUsers
                        .FirstOrDefault(u => u.UserId == userId) != null
                    )
                    .Take(take)
                    .Skip(page * take)
                    .Include(library => library.LibraryMovies
                        .Where(libraryMovie => libraryMovie.Movie.VideoFiles
                            .Any(videoFile => videoFile.Folder != null) == true
                        )
                    )
                    .ThenInclude(libraryMovie => libraryMovie.Movie)
                    .ThenInclude(movie => movie.VideoFiles)
                    .Include(library => library.LibraryMovies)
                    .ThenInclude(libraryMovie => libraryMovie.Movie.Media
                        .Where(media => media.Iso6391 == language || media.Iso6391 == "en"))
                    .Include(library => library.LibraryMovies)
                    .ThenInclude(libraryMovie => libraryMovie.Movie.Images
                        .Where(image => image.Iso6391 == language || image.Iso6391 == "en"))
                    .Include(library => library.LibraryMovies)
                    .ThenInclude(libraryMovie => libraryMovie.Movie.GenreMovies)
                    .ThenInclude(genreMovie => genreMovie.Genre)
                    .Include(library => library.LibraryMovies)
                    .ThenInclude(libraryMovie => libraryMovie.Movie.Translations
                        .Where(translation => translation.Iso6391 == language || translation.Iso6391 == "en"))
                    .Include(library => library.LibraryMovies)
                    .ThenInclude(libraryMovie => libraryMovie.Movie.CertificationMovies)
                    .ThenInclude(certificationMovie => certificationMovie.Certification)
                    .Include(library => library.LibraryTvs
                        .Where(libraryTv => libraryTv.Tv.Episodes
                            .Any(episode => episode.VideoFiles
                                .Any(videoFile => videoFile.Folder != null) == true
                            ) == true
                        )
                    )
                    .ThenInclude(libraryTv => libraryTv.Tv)
                    .ThenInclude(tv => tv.Episodes
                        .Where(episode => episode.SeasonNumber > 0 && episode.VideoFiles.Count != 0))
                    .ThenInclude(episode => episode.VideoFiles)
                    .Include(library => library.LibraryTvs)
                    .ThenInclude(libraryTv => libraryTv.Tv.Media
                        .Where(media => media.Iso6391 == language || media.Iso6391 == "en"))
                    .Include(library => library.LibraryTvs)
                    .ThenInclude(libraryTv => libraryTv.Tv.Images
                        .Where(image => image.Iso6391 == language || image.Iso6391 == "en"))
                    .Include(library => library.LibraryTvs)
                    .ThenInclude(libraryTv => libraryTv.Tv.GenreTvs)
                    .ThenInclude(genreTv => genreTv.Genre)
                    .Include(library => library.LibraryTvs)
                    .ThenInclude(libraryTv => libraryTv.Tv.Translations
                        .Where(translation => translation.Iso6391 == language || translation.Iso6391 == "en"))
                    .Include(library => library.LibraryTvs)
                    .ThenInclude(libraryTv => libraryTv.Tv.CertificationTvs)
                    .ThenInclude(certificationTv => certificationTv.Certification)
                    .First());
}

public record LibraryResponseItemDto
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }
    [JsonProperty("videos")] public VideoDto[] Videos { get; set; }

    public LibraryResponseItemDto(LibraryMovie movie)
    {
        Id = movie.Movie.Id.ToString();
        Backdrop = movie.Movie.Backdrop;
        Logo = movie.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "movie";
        Year = movie.Movie.ReleaseDate.ParseYear();
        Overview = movie.Movie.Overview;
        ColorPalette = movie.Movie.ColorPalette;
        Poster = movie.Movie.Poster;
        Title = movie.Movie.Title;
        TitleSort = movie.Movie.Title
            .TitleSort(movie.Movie.ReleaseDate);
        Type = "movie";

        Genres = movie.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = movie.Movie.Video;
        Videos = movie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public LibraryResponseItemDto(LibraryTv tv)
    {
        Id = tv.Tv.Id.ToString();
        Backdrop = tv.Tv.Backdrop;
        Logo = tv.Tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        Year = tv.Tv.FirstAirDate.ParseYear();
        Overview = tv.Tv.Overview;
        ColorPalette = tv.Tv.ColorPalette;
        Poster = tv.Tv.Poster;
        Title = tv.Tv.Title;
        TitleSort = tv.Tv.Title.TitleSort(tv.Tv.FirstAirDate);

        Type = "tv";
        MediaType = "tv";

        NumberOfItems = tv.Tv.NumberOfEpisodes;
        HaveItems = tv.Tv.Episodes
            .Count(episode => episode.VideoFiles.Any(v => v.Folder != null));

        Genres = tv.Tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        VideoId = tv.Tv.Trailer;
        Videos = tv.Tv.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public LibraryResponseItemDto(Movie movie)
    {
        Id = movie.Id.ToString();
        Backdrop = movie.Backdrop;
        Logo = movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "movie";
        Year = movie.ReleaseDate.ParseYear();
        Overview = movie.Overview;
        ColorPalette = movie.ColorPalette;
        Poster = movie.Poster;
        Title = movie.Title;
        TitleSort = movie.Title
            .TitleSort(movie.ReleaseDate);
        Type = "movie";

        Genres = movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = movie.Video;
        Videos = movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public LibraryResponseItemDto(Tv tv)
    {
        Id = tv.Id.ToString();
        Backdrop = tv.Backdrop;
        Logo = tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        Year = tv.FirstAirDate.ParseYear();
        Overview = tv.Overview;
        ColorPalette = tv.ColorPalette;
        Poster = tv.Poster;
        Title = tv.Title;
        TitleSort = tv.Title.TitleSort(tv.FirstAirDate);

        Type = "tv";
        MediaType = "tv";

        NumberOfItems = tv.NumberOfEpisodes;
        HaveItems = tv.Episodes
            .Count(episode => episode.VideoFiles.Any(v => v.Folder != null));

        Genres = tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        VideoId = tv.Trailer;
        Videos = tv.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public LibraryResponseItemDto(CollectionMovie movie)
    {
        Id = movie.Movie.Id.ToString();
        Backdrop = movie.Movie.Backdrop;
        Logo = movie.Movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "movie";
        Year = movie.Movie.ReleaseDate.ParseYear();
        Overview = movie.Movie.Overview;
        ColorPalette = movie.Movie.ColorPalette;
        Poster = movie.Movie.Poster;
        Title = movie.Movie.Title;
        TitleSort = movie.Movie.Title
            .TitleSort(movie.Movie.ReleaseDate);
        Type = "movie";

        Genres = movie.Movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();
        VideoId = movie.Movie.Video;
        Videos = movie.Movie.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public LibraryResponseItemDto(Collection collection)
    {
        string title = collection.Translations
            .FirstOrDefault()?.Title ?? collection.Title;

        string overview = collection.Translations
            .FirstOrDefault()?.Overview ?? collection.Overview ?? string.Empty;

        Id = collection.Id.ToString();
        Title = title;
        Overview = overview;
        Backdrop = collection.Backdrop;
        Logo = collection.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        Year = collection.CollectionMovies
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate.ParseYear();

        ColorPalette = collection.ColorPalette;
        Poster = collection.Poster;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate.ParseYear());

        Type = "specials";
        MediaType = "specials";

        NumberOfItems = collection.Parts;
        HaveItems = collection.CollectionMovies
            .Count(collectionMovie => collectionMovie.Movie.VideoFiles.Any(v => v.Folder != null));

        Genres = collection.CollectionMovies
            .Select(genreTv => genreTv.Movie)
            .SelectMany(movie => movie.GenreMovies
                .Select(genreMovie => genreMovie.Genre))
            .Select(genre => new GenreDto(genre))
            .ToArray();

        VideoId = collection.CollectionMovies
            .FirstOrDefault()
            ?.Movie.Video;
    }

    public LibraryResponseItemDto(Special special)
    {
        string title = special.Title;

        string overview = special.Overview ?? string.Empty;

        Id = special.Id.ToString();
        Name = title;
        Overview = overview;
        Backdrop = special.Backdrop;
        MediaType = "specials";

        ColorPalette = special.ColorPalette;
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();

        Type = "specials";
    }

    public LibraryResponseItemDto(Person person)
    {
        string name = person.Translations
            .FirstOrDefault()?.Title ?? person.Name;

        string biography = person.Translations
            .FirstOrDefault()?.Biography ?? person.Biography ?? string.Empty;

        Id = person.Id.ToString();
        Name = name;
        Overview = biography;

        MediaType = "person";
        Type = "person";

        TitleSort = person.Name;
        ColorPalette = person.ColorPalette;
        Poster = person.Profile;
    }
}