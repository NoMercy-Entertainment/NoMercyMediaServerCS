using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record GenreResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }

    [JsonProperty("data")] public IOrderedEnumerable<GenreResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, int, string, int, int, Task<Genre>> GetGenre =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id, string language, int take, int page) =>
            mediaContext.Genres.AsNoTracking()
                .Where(genre => genre.Id == id)
                // .Where(genre => genre.GenresUsers
                //     .FirstOrDefault(u => u.UserId == userId) != null
                // )
                .Take(take)
                .Skip(page * take)
                .Include(genre => genre.GenreMovies
                    .Where(genreMovie => genreMovie.Movie.VideoFiles
                        .Any(videoFile => videoFile.Folder != null) == true
                    )
                )
                .ThenInclude(genreMovie => genreMovie.Movie)
                .ThenInclude(movie => movie.VideoFiles)
                .Include(genre => genre.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Movie.Media)
                .Include(genre => genre.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Movie.Images)
                .Include(genre => genre.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Movie.Translations
                    .Where(translation => translation.Iso6391 == language))
                .Include(genre => genre.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Movie.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
                .Include(genre => genre.GenreTvShows
                    .Where(genreTv => genreTv.Tv.Episodes
                        .Any(episode => episode.VideoFiles
                            .Any(videoFile => videoFile.Folder != null) == true
                        ) == true
                    )
                )
                .ThenInclude(genreTv => genreTv.Tv)
                .ThenInclude(tv => tv.Episodes
                    .Where(episode => episode.SeasonNumber > 0 && episode.VideoFiles.Count != 0))
                .ThenInclude(episode => episode.VideoFiles)
                .Include(genre => genre.GenreTvShows)
                .ThenInclude(genreTv => genreTv.Tv.Media)
                .Include(genre => genre.GenreTvShows)
                .ThenInclude(genreTv => genreTv.Tv.Images)
                .Include(genre => genre.GenreTvShows)
                .ThenInclude(genreTv => genreTv.Tv.Translations
                    .Where(translation => translation.Iso6391 == language))
                .Include(genre => genre.GenreTvShows)
                .ThenInclude(genreTv => genreTv.Tv.CertificationTvs)
                .ThenInclude(certificationTv => certificationTv.Certification)
                .First());
}

public record GenreResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
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
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public int? Year { get; set; }
    [JsonProperty("genres")] public GenreDto[]? Genres { get; set; }
    [JsonProperty("videoId")] public string? VideoId { get; set; }
    [JsonProperty("videos")] public VideoDto[] Videos { get; set; }

    public GenreResponseItemDto(GenreMovie movie)
    {
        Id = movie.Movie.Id;
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

    public GenreResponseItemDto(GenreTv tv)
    {
        Id = tv.Tv.Id;
        Backdrop = tv.Tv.Backdrop;
        Logo = tv.Tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;
        MediaType = "tv";
        Year = tv.Tv.FirstAirDate.ParseYear();
        Overview = tv.Tv.Overview;
        ColorPalette = tv.Tv.ColorPalette;
        Poster = tv.Tv.Poster;
        Title = tv.Tv.Title;
        TitleSort = tv.Tv.Title.TitleSort(tv.Tv.FirstAirDate);
        Type = "tv";

        NumberOfItems = tv.Tv.Episodes.Count;
        HaveItems = tv.Tv.Episodes
            .Count(episode => episode.VideoFiles.Any(videoFile => videoFile.Folder != null));

        Genres = tv.Tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv))
            .ToArray();
        VideoId = tv.Tv.Trailer;
        Videos = tv.Tv.Media
            .Where(media => media.Site == "YouTube")
            .Select(media => new VideoDto(media))
            .ToArray();
    }

    public GenreResponseItemDto(CollectionMovie movie)
    {
        Id = movie.Movie.Id;
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

    public GenreResponseItemDto(Collection collection)
    {
        string title = collection.Translations
            .FirstOrDefault()?.Title ?? collection.Title;

        string overview = collection.Translations
            .FirstOrDefault()?.Overview ?? collection.Overview ?? string.Empty;

        Id = collection.Id;
        Title = title;
        Overview = overview;
        Backdrop = collection.Backdrop;
        Logo = collection.Images
            .FirstOrDefault(media => media.Type == "logo")?.FilePath;

        MediaType = "collection";
        Year = collection.CollectionMovies
            .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
            ?.Movie.ReleaseDate.ParseYear();

        ColorPalette = collection.ColorPalette;
        Poster = collection.Poster;
        TitleSort = collection.Title
            .TitleSort(collection.CollectionMovies
                .MinBy(collectionMovie => collectionMovie.Movie.ReleaseDate)
                ?.Movie.ReleaseDate.ParseYear());

        Type = "collection";

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
}