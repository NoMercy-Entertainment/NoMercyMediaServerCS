using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;
using NoMercy.Providers.TMDB.Models.Collections;
using Collection = NoMercy.Database.Models.Collection;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record CollectionResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public CollectionResponseItemDto? Data { get; set; }

    public static readonly Func<MediaContext, Guid, int, string, string, Task<Collection?>> GetCollection =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, int id, string language, string country) =>
            mediaContext.Collections.AsNoTracking()
                .Where(collection => collection.Id == id)
                .Where(collection => collection.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
                .Include(collection => collection.CollectionUser
                    .Where(x => x.UserId == userId)
                )
                .Include(collection => collection.Library)
                .ThenInclude(library => library.LibraryUsers)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Translations
                    .Where(translation => translation.Iso6391 == language))
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.VideoFiles)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.MovieUser
                    .Where(x => x.UserId == userId)
                )
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.CertificationMovies
                    .Where(certificationMovie => certificationMovie.Certification.Iso31661 == "US" ||
                                                 certificationMovie.Certification.Iso31661 == country)
                )
                .ThenInclude(certificationMovie => certificationMovie.Certification)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.GenreMovies)
                .ThenInclude(genreMovie => genreMovie.Genre)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Cast)
                .ThenInclude(genreMovie => genreMovie.Person)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Cast)
                .ThenInclude(genreMovie => genreMovie.Role)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Crew)
                .ThenInclude(genreMovie => genreMovie.Job)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Crew)
                .ThenInclude(genreMovie => genreMovie.Person)
                .Include(collection => collection.CollectionMovies)
                .ThenInclude(movie => movie.Movie)
                .ThenInclude(movie => movie.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                    .OrderByDescending(image => image.VoteAverage)
                    .Take(30)
                )
                .Include(collection => collection.Translations
                    .Where(translation => translation.Iso6391 == language))
                .Include(collection => collection.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                    .OrderByDescending(image => image.VoteAverage)
                )
                .FirstOrDefault());
}

public record CollectionResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("duration")] public double Duration { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("collection")] public CollectionMovieDto[] Collection { get; set; }
    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("genres")] public GenreDto[] Genres { get; set; }
    [JsonProperty("total_duration")] public int TotalDuration { get; set; }

    [JsonProperty("cast")] public PeopleDto[] Cast { get; set; }
    [JsonProperty("crew")] public PeopleDto[] Crew { get; set; }
    [JsonProperty("backdrops")] public ImageDto[] Backdrops { get; set; }
    [JsonProperty("posters")] public ImageDto[] Posters { get; set; }

    [JsonProperty("content_ratings")] public ContentRating[] ContentRatings { get; set; }

    public CollectionResponseItemDto(Collection collection, string? country)
    {
        string? title = collection.Translations.FirstOrDefault()?.Title;
        string? overview = collection.Translations.FirstOrDefault()?.Overview;

        Id = collection.Id;
        Title = !string.IsNullOrEmpty(title)
            ? title
            : collection.Title;
        Overview = !string.IsNullOrEmpty(overview)
            ? overview
            : collection.Overview;
        Backdrop = collection.Backdrop;
        Poster = collection.Poster;
        TitleSort = collection.TitleSort;

        Type = "collection";
        MediaType = "collection";

        ColorPalette = collection.ColorPalette;
        NumberOfItems = collection.Parts;
        HaveItems = collection.CollectionMovies.Count(collectionMovie => collectionMovie.Movie.VideoFiles.Count > 0);

        TotalDuration = collection.CollectionMovies.Sum(item => item.Movie.Runtime * 60 ?? 0);

        Favorite = collection.CollectionUser.Count != 0;
        Watched = collection.CollectionMovies.Count(collectionMovie => collectionMovie.Movie.MovieUser.Count != 0) ==
                  collection.CollectionMovies.Count;

        Duration = collection.CollectionMovies
            .Select(movie => movie.Movie.Runtime)
            .Average() ?? 0;

        Genres = collection.CollectionMovies
            .SelectMany(movie => movie.Movie.GenreMovies)
            .DistinctBy(genreMovie => genreMovie.GenreId)
            .Select(genreMovie => new GenreDto(genreMovie))
            .ToArray();

        ContentRatings = collection.CollectionMovies
            .SelectMany(collectionMovie => collectionMovie.Movie.CertificationMovies)
            .DistinctBy(certification => certification.Certification.Iso31661)
            .Select(certificationMovie => new ContentRating
            {
                Rating = certificationMovie.Certification.Rating,
                Iso31661 = certificationMovie.Certification.Iso31661
            })
            .ToArray();

        Collection = collection.CollectionMovies
            .OrderBy(movie => movie.Movie.ReleaseDate)
            .Select(movie => new CollectionMovieDto(movie.Movie))
            .ToArray();

        Backdrops = collection.CollectionMovies
            .SelectMany(movie => movie.Movie.Images)
            .Where(media => media.Type == "backdrop")
            .Select(media => new ImageDto(media))
            .OrderByDescending(image => image.VoteAverage)
            .ToArray();

        Posters = collection.CollectionMovies
            .SelectMany(movie => movie.Movie.Images)
            .Where(media => media.Type == "poster")
            .Select(media => new ImageDto(media))
            .OrderByDescending(image => image.VoteAverage)
            .ToArray();

        Cast = collection.CollectionMovies
            .SelectMany(movie => movie.Movie.Cast)
            .Select(cast => new PeopleDto(cast))
            .OrderBy(cast => cast.Order)
            .ToArray();

        Crew = collection.CollectionMovies
            .SelectMany(movie => movie.Movie.Crew)
            .Select(crew => new PeopleDto(crew))
            .OrderBy(crew => crew.Order)
            .ToArray();
    }

    public CollectionResponseItemDto(TmdbCollectionAppends tmdbCollectionAppends)
    {
        string? title = tmdbCollectionAppends.Translations.Translations.FirstOrDefault()?.Data.Title;
        string? overview = tmdbCollectionAppends.Translations.Translations.FirstOrDefault()?.Data.Overview;

        Id = tmdbCollectionAppends.Id;
        Title = !string.IsNullOrEmpty(title)
            ? title
            : tmdbCollectionAppends.Name;
        Overview = !string.IsNullOrEmpty(overview)
            ? overview
            : tmdbCollectionAppends.Overview;
        Id = tmdbCollectionAppends.Id;
        Title = tmdbCollectionAppends.Name;
        Overview = tmdbCollectionAppends.Overview;
        Backdrop = tmdbCollectionAppends.BackdropPath;
        Poster = tmdbCollectionAppends.PosterPath;
        TitleSort = tmdbCollectionAppends.Name.TitleSort();
        Type = "collection";
        MediaType = "collection";
        ColorPalette = new IColorPalettes();
        NumberOfItems = tmdbCollectionAppends.Parts.Length;
        HaveItems = 0;
        Favorite = false;

        Collection = tmdbCollectionAppends.Parts
            .Select(movie => new CollectionMovieDto(movie))
            .ToArray();

        Backdrops = tmdbCollectionAppends.Images.Backdrops
            .Select(media => new ImageDto(media))
            .OrderByDescending(image => image.VoteAverage)
            .ToArray();
        Posters = tmdbCollectionAppends.Images.Posters
            .Select(media => new ImageDto(media))
            .OrderByDescending(image => image.VoteAverage)
            .ToArray();
    }
}

public record CollectionMovieDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("watched")] public bool Watched { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("year")] public long Year { get; set; }
    [JsonProperty("genres")] public GenreDto[] Genres { get; set; }

    [JsonProperty("rating")] public Certification? Rating { get; set; }

    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }

    public CollectionMovieDto(Movie movie)
    {
        string? title = movie.Translations.FirstOrDefault()?.Title;
        string? overview = movie.Translations.FirstOrDefault()?.Overview;

        Id = movie.Id;
        Title = !string.IsNullOrEmpty(title)
            ? title
            : movie.Title;
        Overview = !string.IsNullOrEmpty(overview)
            ? overview
            : movie.Overview;

        Backdrop = movie.Backdrop;
        Favorite = movie.MovieUser.Count != 0;
        // Watched = movie.Watched;
        Logo = movie.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;

        MediaType = "movie";
        ColorPalette = movie.ColorPalette;
        Poster = movie.Poster;
        Type = "movie";
        Year = movie.ReleaseDate.ParseYear();
        Genres = movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie.Genre))
            .ToArray();

        Rating = movie.CertificationMovies
            .Select(certificationMovie => certificationMovie.Certification)
            .FirstOrDefault();

        NumberOfItems = 1;
        HaveItems = movie.VideoFiles.Count > 0 ? 1 : 0;

        VideoId = movie.Video;
    }

    public CollectionMovieDto(Providers.TMDB.Models.Movies.TmdbMovie tmdbMovie)
    {
        Id = tmdbMovie.Id;
        Title = tmdbMovie.Title;
        Overview = tmdbMovie.Overview;
        Id = tmdbMovie.Id;
        Title = tmdbMovie.Title;
        Overview = tmdbMovie.Overview;
        Backdrop = tmdbMovie.BackdropPath;
        Favorite = false;
        Watched = false;
        // Logo = movie.LogoPath;
        MediaType = "movie";
        ColorPalette = new IColorPalettes();
        Poster = tmdbMovie.PosterPath;
        Type = "movie";
        Year = tmdbMovie.ReleaseDate.ParseYear();

        NumberOfItems = 1;
        HaveItems = 0;

        VideoId = tmdbMovie.Video;
    }
}