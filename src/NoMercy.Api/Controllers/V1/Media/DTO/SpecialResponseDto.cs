#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;
using Special = NoMercy.Database.Models.Special;

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record SpecialResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public SpecialResponseItemDto? Data { get; set; }

    #region GetSpecial

    public static readonly Func<MediaContext, Guid, Ulid, string, string, Task<Special?>> GetSpecial =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id, string language, string country) =>
            mediaContext.Specials
                .AsNoTracking()
                .Where(special => special.Id == id)
                .Include(special => special.Items
                    .OrderBy(specialItem => specialItem.Order)
                )
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.VideoFiles)
                .ThenInclude(file => file.UserData
                    .Where(userData => userData.UserId == userId)
                )
                .Include(special => special.Items
                    .OrderBy(specialItem => specialItem.Order)
                )
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(movie => movie!.VideoFiles)
                .ThenInclude(file => file.UserData
                    .Where(userData => userData.UserId == userId)
                )
                .Include(special => special.SpecialUser
                    .Where(specialUser => specialUser.UserId == userId)
                )
                .FirstOrDefault());

    #endregion

    #region GetSpecialMovies

    public static readonly Func<MediaContext, Guid, IEnumerable<int>, string, string, IAsyncEnumerable<Movie>>
        GetSpecialMovies =
            EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, IEnumerable<int> ids, string language,
                    string country) =>
                mediaContext.Movies.AsNoTracking()
                    .Where(movie => ids.Contains(movie.Id))
                    .Include(movie => movie.Translations
                        .Where(translation => translation.Iso6391 == language)
                    )
                    .Include(movie => movie.MovieUser
                        .Where(movieUser => movieUser.UserId == userId)
                    )
                    .Include(movie => movie.CertificationMovies
                        .Where(certification => certification.Certification.Iso31661 == country ||
                                                certification.Certification.Iso31661 == "US")
                    )
                    .ThenInclude(certificationMovie => certificationMovie.Certification)
                    .Include(movie => movie.VideoFiles)
                    .ThenInclude(file => file.UserData
                        .Where(userData => userData.UserId == userId)
                    )
                    .Include(movie => movie.GenreMovies)
                    .ThenInclude(genreMovie => genreMovie.Genre)
                    .Include(movie => movie.Cast
                        .OrderBy(castTv => castTv.Role.Order)
                    )
                    .ThenInclude(castTv => castTv.Person)
                    .Include(movie => movie.Cast
                        .OrderBy(castTv => castTv.Role.Order)
                    )
                    .ThenInclude(castTv => castTv.Role)
                    .Include(movie => movie.Crew)
                    .ThenInclude(crew => crew.Person)
                    .Include(movie => movie.Crew)
                    .ThenInclude(crew => crew.Job)
                    .Include(movie => movie.Images
                        .Where(image =>
                            (image.Type == "logo" && image.Iso6391 == "en")
                            || ((image.Type == "backdrop" || image.Type == "poster") &&
                                (image.Iso6391 == "en" || image.Iso6391 == null))
                        )
                        .OrderByDescending(image => image.VoteAverage)
                    )
            );

    #endregion

    #region GetSpecialTvs

    public static readonly Func<MediaContext, Guid, IEnumerable<int>, string, string, IAsyncEnumerable<Tv>>
        GetSpecialTvs =
            EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, IEnumerable<int> ids, string language,
                    string country) =>
                mediaContext.Tvs.AsNoTracking()
                    .Where(tv => tv.Episodes.Any(e => ids.Contains(e.Id)))
                    .Include(tv => tv.Translations
                        .Where(translation => translation.Iso6391 == language)
                    )
                    .Include(tv => tv.TvUser
                        .Where(tvUser => tvUser.UserId == userId)
                    )
                    .Include(tv => tv.GenreTvs)
                    .ThenInclude(genreTv => genreTv.Genre)
                    .Include(tv => tv.Cast
                        .OrderBy(castTv => castTv.Role.Order)
                    )
                    .ThenInclude(castTv => castTv.Person)
                    .Include(tv => tv.Cast
                        .OrderBy(castTv => castTv.Role.Order)
                    )
                    .ThenInclude(castTv => castTv.Role)
                    .Include(tv => tv.Crew)
                    .ThenInclude(crew => crew.Person)
                    .Include(tv => tv.Crew)
                    .ThenInclude(crew => crew.Job)
                    .Include(tv => tv.CertificationTvs
                        .Where(certification => certification.Certification.Iso31661 == country ||
                                                certification.Certification.Iso31661 == "US")
                    )
                    .ThenInclude(certificationTv => certificationTv.Certification)
                    .Include(tv => tv.Images
                        .Where(image =>
                            (image.Type == "logo" && image.Iso6391 == "en")
                            || ((image.Type == "backdrop" || image.Type == "poster") &&
                                (image.Iso6391 == "en" || image.Iso6391 == null))
                        )
                        .OrderByDescending(image => image.VoteAverage)
                    )
                    .Include(tv => tv.Episodes)
                    .ThenInclude(episode => episode.VideoFiles)
                    .ThenInclude(file => file.UserData
                        .Where(userData => userData.UserId == userId))
            );

    #endregion

    #region GetSpecialAvailable

    public static readonly Func<MediaContext, Guid, Ulid, Task<Special?>> GetSpecialAvailable =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id) =>
            mediaContext.Specials.AsNoTracking()
                .Where(special => special.Id == id)
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.VideoFiles)
                .ThenInclude(file => file.UserData)
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(movie => movie!.VideoFiles)
                .ThenInclude(file => file.UserData)
                .FirstOrDefault());

    #endregion

    #region GetSpecialPlaylist

    public static readonly Func<MediaContext, Guid, Ulid, string, Task<Special?>> GetSpecialPlaylist =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, Ulid id, string language) =>
            mediaContext.Specials.AsNoTracking()
                .Where(special => special.Id == id)
                .Include(special => special.Items
                    .OrderBy(specialItem => specialItem.Order)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.VideoFiles)
                .ThenInclude(file => file.UserData
                    .Where(userData => userData.UserId == userId)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.CertificationMovies)
                .ThenInclude(certificationMovie => certificationMovie.Certification)
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.MovieUser
                    .Where(movieUser => movieUser.UserId == userId)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Movie)
                .ThenInclude(movie => movie!.Translations
                    .Where(translation => translation.Iso6391 == language)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.VideoFiles)
                .ThenInclude(file => file.UserData
                    .Where(userData => userData.UserId == userId)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Season)
                .ThenInclude(episode => episode.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Translations
                    .Where(translation => translation.Iso6391 == language)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Tv)
                .ThenInclude(episode => episode.Translations
                    .Where(translation => translation.Iso6391 == language)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Tv)
                .ThenInclude(episode => episode.CertificationTvs)
                .ThenInclude(certificationTv => certificationTv.Certification)
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Tv)
                .ThenInclude(episode => episode.Images
                    .Where(image =>
                        (image.Type == "logo" && image.Iso6391 == "en")
                        || ((image.Type == "backdrop" || image.Type == "poster") &&
                            (image.Iso6391 == "en" || image.Iso6391 == null))
                    )
                    .OrderByDescending(image => image.VoteAverage)
                )
                .Include(special => special.Items)
                .ThenInclude(specialItem => specialItem.Episode)
                .ThenInclude(episode => episode!.Tv)
                .ThenInclude(episode => episode.TvUser
                    .Where(tvUser => tvUser.UserId == userId)
                )
                .FirstOrDefault());

    public SpecialResponseDto(Special special)
    {
    }

    public SpecialResponseDto()
    {
        //
    }

    #endregion
}

public record SpecialResponseItemDto
{
    [JsonProperty("id")] public Ulid Id { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("collection")] public IEnumerable<SpecialItemDto>? Special { get; set; }
    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("genres")] public IEnumerable<GenreDto> Genres { get; set; }
    [JsonProperty("total_duration")] public int TotalDuration { get; set; }

    [JsonProperty("cast")] public IEnumerable<PeopleDto> Cast { get; set; }
    [JsonProperty("crew")] public IEnumerable<PeopleDto> Crew { get; set; }
    [JsonProperty("backdrops")] public IEnumerable<ImageDto> Backdrops { get; set; }
    [JsonProperty("posters")] public IEnumerable<ImageDto> Posters { get; set; }

    [JsonProperty("content_ratings")] public IEnumerable<Certification?> ContentRatings { get; set; }

    public SpecialResponseItemDto(Special special, List<SpecialItemsDto> items)
    {
        List<SpecialItemDto> specialItems = [];
        foreach (SpecialItem specialItem in special.Items)
            if (specialItem.MovieId is not null)
            {
                SpecialItemsDto? newItem = items.Find(i => i.Id == specialItem.MovieId);
                if (newItem is null) continue;

                SpecialItemDto item = new(newItem);
                specialItems.Add(item);
            }
            else
            {
                SpecialItemsDto? newItem = items.FirstOrDefault(i => i.EpisodeIds.Contains(specialItem.EpisodeId ?? 0));
                if (newItem is null) continue;

                SpecialItemDto item = new(newItem);
                specialItems.Add(item);
            }

        IEnumerable<PeopleDto> cast = items
            .SelectMany(tv => tv.Cast)
            .ToList();

        IEnumerable<PeopleDto> crew = items
            .SelectMany(item => item.Crew)
            .ToList();

        IEnumerable<ImageDto> posters = items
            .SelectMany(item => item.Posters)
            .ToList();

        IEnumerable<ImageDto> backdrops = items
            .SelectMany(item => item.Backdrops)
            .ToList();

        IEnumerable<GenreDto> genres = items
            .SelectMany(item => item.Genres)
            .ToList();

        foreach (SpecialItemsDto item in items)
        {
            item.Posters = [];
            item.Backdrops = [];
            item.Cast = [];
            item.Crew = [];
            item.Genres = [];
        }

        Id = special.Id;
        Title = special.Title;
        Overview = special.Description;
        Backdrop = special.Backdrop?.Replace("https://storage.nomercy.tv/laravel", "");
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();
        Type = "specials";
        MediaType = "specials";
        ColorPalette = special.ColorPalette;
        Backdrops = backdrops;
        Posters = posters;
        Cast = cast;
        Crew = crew;
        Genres = genres.DistinctBy(genre => genre.Id);

        Favorite = special.SpecialUser.Count != 0;

        NumberOfItems = special.Items.Count;

        int movies = special.Items.Count(item => item.MovieId is not null && item.Movie?.VideoFiles.Count != 0);
        int episodes = special.Items.Count(item => item.EpisodeId is not null && item.Episode?.VideoFiles.Count != 0);

        HaveItems = movies + episodes;

        TotalDuration = items.Sum(item => item.Duration);

        ContentRatings = items
            .Select(specialItem => specialItem.Rating)
            .DistinctBy(rating => rating.Iso31661);

        Special = specialItems.DistinctBy(si => si.Id);
    }

    public SpecialResponseItemDto(Special special)
    {
        Id = special.Id;
        Title = special.Title;
        Overview = special.Description;
        Backdrop = special.Backdrop?.Replace("https://storage.nomercy.tv/laravel", "");
        Poster = special.Poster;
        TitleSort = special.Title.TitleSort();
        Type = "specials";
        MediaType = "specials";
        ColorPalette = special.ColorPalette;
        Favorite = special.SpecialUser?.Count != 0;
        NumberOfItems = special.Items.Count;

        int movies = special.Items.Count(item => item.MovieId is not null && (bool)item.Movie?.VideoFiles.Any());
        int episodes = special.Items.Count(item => item.EpisodeId is not null && (bool)item.Episode?.VideoFiles.Any());

        HaveItems = movies + episodes;

        TotalDuration = special.Items.Sum(item => item.Movie?.Runtime ?? 0);

        ContentRatings = special.Items
            .Select(specialItem => specialItem.Movie?.CertificationMovies
                .Select(certification => certification.Certification)
                .FirstOrDefault())
            .DistinctBy(rating => rating?.Iso31661);
    }
}

public record SpecialItemsDto
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("episode_ids")] public int[] EpisodeIds { get; set; }
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
    [JsonProperty("backdrops")] public IEnumerable<ImageDto> Backdrops { get; set; }
    [JsonProperty("posters")] public IEnumerable<ImageDto> Posters { get; set; }

    [JsonProperty("cast")] public IEnumerable<PeopleDto> Cast { get; set; }
    [JsonProperty("crew")] public IEnumerable<PeopleDto> Crew { get; set; }

    [JsonProperty("rating")] public Certification Rating { get; set; }

    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int HaveItems { get; set; }
    [JsonProperty("duration")] public int Duration { get; set; }

    public SpecialItemsDto()
    {
        //
    }

    public SpecialItemsDto(Movie movie)
    {
        string? title = movie.Translations.FirstOrDefault()?.Title;
        string? overview = movie.Translations.FirstOrDefault()?.Overview;

        Id = movie.Id;
        EpisodeIds = [];
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

        Backdrops = movie.Images
            .Where(media => media.Type == "backdrop")
            .Take(2)
            .Select(media => new ImageDto(media));

        Posters = movie.Images
            .Where(media => media.Type == "poster")
            .Take(2)
            .Select(media => new ImageDto(media));

        MediaType = "movie";
        ColorPalette = movie.ColorPalette;
        Poster = movie.Poster;
        Type = "movie";
        Year = movie.ReleaseDate.ParseYear();
        Duration = movie.Runtime * 60 ?? 0;

        Genres = movie.GenreMovies
            .Select(genreMovie => new GenreDto(genreMovie.Genre))
            .ToArray();

        Rating = movie.CertificationMovies
            .Select(certificationMovie => certificationMovie.Certification)
            .FirstOrDefault() ?? new Certification();

        NumberOfItems = 1;
        HaveItems = movie.VideoFiles.Count > 0 ? 1 : 0;

        VideoId = movie.Video;

        Cast = movie.Cast
            .Take(15)
            .Select(cast => new PeopleDto(cast));

        Crew = movie.Crew
            .Take(15)
            .Select(crew => new PeopleDto(crew));
    }

    public SpecialItemsDto(Tv tv)
    {
        string? title = tv.Translations.FirstOrDefault()?.Title;
        string? overview = tv.Translations.FirstOrDefault()?.Overview;

        Id = tv.Id;
        EpisodeIds = tv.Episodes?
            .Select(episode => episode.Id)
            .ToArray() ?? [];

        Title = !string.IsNullOrEmpty(title)
            ? title
            : tv.Title;
        Overview = !string.IsNullOrEmpty(overview)
            ? overview
            : tv.Overview;

        Backdrop = tv.Backdrop;
        Favorite = tv.TvUser.Count != 0;
        // Watched = tv.Watched;
        Logo = tv.Images
            .FirstOrDefault(media => media.Type == "logo")
            ?.FilePath;

        Backdrops = tv.Images
            .Where(media => media.Type == "backdrop")
            .Take(2)
            .Select(media => new ImageDto(media));

        Posters = tv.Images
            .Where(media => media.Type == "poster")
            .Take(2)
            .Select(media => new ImageDto(media));

        MediaType = "tv";
        ColorPalette = tv.ColorPalette;
        Poster = tv.Poster;
        Type = "tv";
        Year = tv.FirstAirDate.ParseYear();

        Genres = tv.GenreTvs
            .Select(genreTv => new GenreDto(genreTv.Genre))
            .ToArray();

        Rating = tv.CertificationTvs
            .Select(certificationTv => certificationTv.Certification)
            .FirstOrDefault() ?? new Certification();

        NumberOfItems = tv.Episodes?.Where(e => e.SeasonNumber > 0).Count() ?? 0;
        int have = tv.Episodes?.Where(e => e.SeasonNumber > 0).Count(episode => episode.VideoFiles.Any()) ?? 0;

        HaveItems = have;

        Duration = tv.Duration * have * 60 ?? 0;

        // Watched = tv.Episodes
        //     .SelectMany(episode => episode!.VideoFiles
        //         .Where(videoFile => videoFile.UserData.Any(userData => userData.UserId == userId)))
        //     .Count();

        VideoId = tv.Trailer;

        Cast = tv.Cast
            .Take(15)
            .Select(cast => new PeopleDto(cast));

        Crew = tv.Crew
            .Take(15)
            .Select(crew => new PeopleDto(crew));
    }
}

public record SpecialItemDto
{
    [JsonProperty("id")] public int Id { get; set; }
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
    [JsonProperty("duration")] public int Duration { get; set; }

    [JsonProperty("rating")] public Certification? Rating { get; set; }

    [JsonProperty("videoId")] public string? VideoId { get; set; }

    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int HaveItems { get; set; }

    public SpecialItemDto(SpecialItemsDto item)
    {
        Id = item.Id;
        Title = item.Title;
        Overview = item.Overview;
        Backdrop = item.Backdrop;
        Favorite = item.Favorite;
        Logo = item.Logo;
        Genres = item.Genres;
        MediaType = item.MediaType;
        ColorPalette = item.ColorPalette;
        Poster = item.Poster;
        Type = item.Type;
        Year = item.Year;
        Rating = item.Rating;
        NumberOfItems = item.NumberOfItems;
        HaveItems = item.HaveItems;
        VideoId = item.VideoId;
        Duration = item.Duration;
    }
}