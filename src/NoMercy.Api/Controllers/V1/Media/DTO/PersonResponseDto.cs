#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;
using NoMercy.Providers.TMDB.Models.Combined;
using NoMercy.Providers.TMDB.Models.People;
using Person = NoMercy.Database.Models.Person;
using TmdbGender = NoMercy.Providers.TMDB.Models.People.TmdbGender;

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record PersonResponseDto
{
    [JsonProperty("nextId")] public long NextId { get; set; }
    [JsonProperty("data")] public PersonResponseItemDto? Data { get; set; }
}

public record PersonResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("also_known_as")] public string[]? AlsoKnownAs { get; set; }
    [JsonProperty("biography")] public string? Biography { get; set; }
    [JsonProperty("birthday")] public DateTime? Birthday { get; set; }
    [JsonProperty("deathday")] public DateTime? DeathDay { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
    [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("place_of_birth")] public string? PlaceOfBirth { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("profile")] public string? Profile { get; set; }
    [JsonProperty("titleSort")] public string TitleSort { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }

    [JsonProperty("combined_credits")] public Credits CombinedCredits { get; set; }

    [JsonProperty("external_ids")] public Database.Models.TmdbPersonExternalIds? ExternalIds { get; set; }
    [JsonProperty("translations")] public TranslationsDto TranslationsDto { get; set; }
    [JsonProperty("known_for")] public KnownFor[] KnownFor { get; set; }
    [JsonProperty("images")] public Images Images { get; set; }

    public PersonResponseItemDto(Person person)
    {
        string? biography = person.Translations
            .FirstOrDefault()?.Biography;

        Id = person.Id;
        Name = person.Name;
        Biography = !string.IsNullOrEmpty(biography)
            ? biography
            : person.Biography;
        Adult = person.Adult;
        AlsoKnownAs = person.AlsoKnownAs is null
            ? []
            : JsonConvert.DeserializeObject<string[]>(person.AlsoKnownAs);
        Birthday = person.BirthDay;
        DeathDay = person.DeathDay;
        Homepage = person.Homepage;
        ImdbId = person.ImdbId;
        KnownForDepartment = person.KnownForDepartment;
        PlaceOfBirth = person.PlaceOfBirth;
        Popularity = person.Popularity;
        Profile = person.Profile;
        ColorPalette = person.ColorPalette;
        CreatedAt = person.CreatedAt;
        UpdatedAt = person.UpdatedAt;
        ExternalIds = person.ExternalIds;
        Gender = person.Gender;

        Images = new Images
        {
            Profiles = person.Images
                .Select(image => new ImageDto(image))
                .ToArray()
        };

        CombinedCredits = new Credits
        {
            Cast = person.Casts
                .Select(cast => new KnownFor(cast))
                .OrderByDescending(knownFor => knownFor.Year)
                .ToArray(),

            Crew = person.Crews
                .Select(crew => new KnownFor(crew))
                .OrderByDescending(knownFor => knownFor.Year)
                .ToArray()
        };

        KnownFor = person.Casts
            .Select(crew => new KnownFor(crew))
            .Concat(person.Crews
                .Select(crew => new KnownFor(crew)))
            .OrderByDescending(knownFor => knownFor.Popularity)
            .ToArray();
    }

    public PersonResponseItemDto(TmdbPersonAppends tmdbPersonAppends, string? country)
    {
        string? biography = tmdbPersonAppends.Translations.Translations
            .FirstOrDefault(translation => translation.Iso31661 == country)?.TmdbPersonTranslationData.Overview;

        using MediaContext context = new();

        Person? person = context.People
            .Where(p => p.Id == tmdbPersonAppends.Id)
            .Include(p => p.Casts)
            .ThenInclude(c => c.Movie)
            .ThenInclude(movie => movie!.VideoFiles)
            .Include(p => p.Casts)
            .ThenInclude(c => c.Tv)
            .ThenInclude(tv => tv!.Episodes)
            .ThenInclude(episode => episode.VideoFiles)
            .Include(p => p.Crews)
            .ThenInclude(c => c.Movie)
            .ThenInclude(movie => movie!.VideoFiles)
            .Include(p => p.Crews)
            .ThenInclude(c => c.Tv)
            .ThenInclude(tv => tv!.Episodes)
            .ThenInclude(episode => episode.VideoFiles)
            .FirstOrDefault();

        Id = tmdbPersonAppends.Id;
        Name = tmdbPersonAppends.Name;
        Biography = !string.IsNullOrEmpty(biography)
            ? biography
            : tmdbPersonAppends.Biography;
        Adult = tmdbPersonAppends.Adult;
        AlsoKnownAs = tmdbPersonAppends.AlsoKnownAs;
        Birthday = tmdbPersonAppends.BirthDay;
        DeathDay = tmdbPersonAppends.DeathDay;
        Homepage = tmdbPersonAppends.Homepage?.ToString();
        ImdbId = tmdbPersonAppends.ImdbId;
        KnownForDepartment = tmdbPersonAppends.KnownForDepartment;
        PlaceOfBirth = tmdbPersonAppends.PlaceOfBirth;
        Popularity = tmdbPersonAppends.Popularity;
        Profile = tmdbPersonAppends.ProfilePath;
        ColorPalette = new IColorPalettes();
        ExternalIds = tmdbPersonAppends.ExternalIds;
        Gender = Enum.Parse<TmdbGender>(tmdbPersonAppends.TmdbGender.ToString(), true).ToString();

        Images = new Images
        {
            Profiles = tmdbPersonAppends.Images.Profiles
                .Select(image => new ImageDto(image))
                .ToArray()
        };

        CombinedCredits = new Credits
        {
            Cast = tmdbPersonAppends.CombinedCredits.Cast
                .Select(cast => new KnownFor(cast, person))
                .OrderByDescending(knownFor => knownFor.Year)
                .ToArray(),

            Crew = tmdbPersonAppends.CombinedCredits.Crew
                .Select(crew => new KnownFor(crew, person))
                .OrderByDescending(knownFor => knownFor.Year)
                .ToArray()
        };

        KnownFor[] cast = tmdbPersonAppends.CombinedCredits.Cast
            .Select(cast => new KnownFor(cast, person))
            .DistinctBy(knownFor => knownFor.Id)
            .ToArray();

        KnownFor[] crew = tmdbPersonAppends.CombinedCredits.Crew
            .Select(crew => new KnownFor(crew, person))
            .DistinctBy(knownFor => knownFor.Id)
            .ToArray();

        KnownFor = cast.Concat(crew)
            .OrderByDescending(knownFor => knownFor.VoteCount)
            .ToArray();
    }
}

public record Credits
{
    [JsonProperty("cast")] public KnownFor[] Cast { get; set; }
    [JsonProperty("crew")] public KnownFor[] Crew { get; set; }
}

public record KnownFor
{
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("genre_ids")] public int[]? GenreIds { get; set; }
    [JsonProperty("id")] public int? Id { get; set; }
    [JsonProperty("original_language")] public string OriginalLanguage { get; set; }
    [JsonProperty("original_title")] public string OriginalTitle { get; set; }
    [JsonProperty("overview")] public string Overview { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("release_date")] public DateTime? ReleaseDate { get; set; }
    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("video")] public bool? Video { get; set; }
    [JsonProperty("vote_average")] public double VoteAverage { get; set; }
    [JsonProperty("vote_count")] public long VoteCount { get; set; }
    [JsonProperty("character")] public string? Character { get; set; }
    [JsonProperty("credit_id")] public string CreditId { get; set; }
    [JsonProperty("order")] public long? Order { get; set; }
    [JsonProperty("media_type")] public string? MediaType { get; set; }
    [JsonProperty("hasItem")] public bool? HasItem { get; set; }
    [JsonProperty("poster")] public string Poster { get; set; }
    [JsonProperty("year")] public long? Year { get; set; }
    [JsonProperty("origin_country")] public string[] OriginCountry { get; set; }
    [JsonProperty("original_name")] public string OriginalName { get; set; }
    [JsonProperty("first_air_date")] public DateTimeOffset? FirstAirDate { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("department")] public string? Department { get; set; }
    [JsonProperty("job")] public string? Job { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("number_of_items")] public int? NumberOfItems { get; set; }
    [JsonProperty("have_items")] public int? HaveItems { get; set; }
    [JsonProperty("episode_count")] public int? EpisodeCount { get; set; }

    public KnownFor(Cast cast)
    {
        Character = cast.Role.Character;
        Title = cast.Movie?.Title ?? cast.Tv?.Title;
        MediaType = cast.Movie is not null ? "movie" : "tv";
        Year = cast.Movie?.ReleaseDate.ParseYear() ?? cast.Tv?.FirstAirDate.ParseYear();
        Id = cast.Movie?.Id ?? cast.Tv?.Id;
        Adult = cast.Movie?.Adult ?? false;
        OriginalLanguage = cast.Movie?.OriginalLanguage ?? cast.Tv?.OriginalLanguage ?? string.Empty;
        Overview = cast.Movie?.Overview ?? cast.Tv?.Overview ?? string.Empty;
        Popularity = cast.Movie?.Popularity ?? cast.Tv?.Popularity ?? 0;
        Poster = cast.Movie?.Poster ?? cast.Tv?.Poster ?? string.Empty;
        Backdrop = cast.Movie?.Backdrop ?? cast.Tv?.Backdrop;
        ReleaseDate = cast.Movie?.ReleaseDate ?? cast.Tv?.FirstAirDate;
        VoteAverage = cast.Movie?.VoteAverage ?? cast.Tv?.VoteAverage ?? 0;
        VoteCount = cast.Movie?.VoteCount ?? cast.Tv?.VoteCount ?? 0;
        HasItem = cast.Movie?.VideoFiles.Count != 0 || (cast.Tv?.Episodes.Any(e => e.VideoFiles.Count != 0) ?? false);
        NumberOfItems = cast.Movie?.VideoFiles.Count + cast.Tv?.Episodes.Count(e => e.VideoFiles.Count != 0);
        HaveItems = cast.Movie?.VideoFiles.Count != 0 ? 1 : cast.Tv?.Episodes.Count(e => e.VideoFiles.Count != 0) ?? 0;
    }

    public KnownFor(Crew crew)
    {
        Title = crew.Movie?.Title ?? crew.Tv!.Title;
        MediaType = crew.Movie is not null ? "movie" : "tv";
        Year = crew.Movie?.ReleaseDate.ParseYear() ?? crew.Tv!.FirstAirDate.ParseYear();
        Id = crew.Movie?.Id ?? crew.Tv!.Id;
        Adult = crew.Movie?.Adult ?? false;
        Backdrop = crew.Movie?.Backdrop ?? crew.Tv!.Backdrop;
        OriginalLanguage = crew.Movie?.OriginalLanguage ?? crew.Tv!.OriginalLanguage ?? string.Empty;
        Overview = crew.Movie?.Overview ?? crew.Tv!.Overview ?? string.Empty;
        Popularity = crew.Movie?.Popularity ?? crew.Tv!.Popularity ?? 0;
        Poster = crew.Movie?.Poster ?? crew.Tv!.Poster ?? string.Empty;
        ReleaseDate = crew.Movie?.ReleaseDate ?? crew.Tv!.FirstAirDate;
        VoteAverage = crew.Movie?.VoteAverage ?? crew.Tv!.VoteAverage ?? 0;
        VoteCount = crew.Movie?.VoteCount ?? crew.Tv!.VoteCount ?? 0;
        Job = crew.Job.Task ?? string.Empty;
        HasItem = crew.Movie?.VideoFiles.Count != 0 || (crew.Tv?.Episodes.Any(e => e.VideoFiles.Count != 0) ?? false);
        NumberOfItems = crew.Movie?.VideoFiles.Count + crew.Tv?.Episodes.Count(e => e.VideoFiles.Count > 0);
        HaveItems = crew.Movie?.VideoFiles.Count != 0 ? 1 : crew.Tv?.Episodes.Count(e => e.VideoFiles.Count > 0) ?? 0;
    }

    public KnownFor(TmdbPersonCredit crew, Person? person)
    {
        int year = crew.ReleaseDate.ParseYear();
        if (year == 0) year = crew.FirstAirDate.ParseYear();

        Character = crew.Character;
        Title = crew.Title ?? crew.Name;
        Backdrop = crew.BackdropPath;
        MediaType = crew.MediaType;
        Type = crew.MediaType;
        Id = crew.Id;
        HasItem = false;
        Adult = crew.Adult;
        Popularity = crew.Popularity;
        Character = crew.Character;
        Job = crew.Job;
        Department = crew.Department;
        Year = year;
        OriginalLanguage = crew.OriginalLanguage;
        Overview = crew.Overview;
        Popularity = crew.Popularity;
        Poster = crew.PosterPath;
        VoteAverage = crew.VoteAverage;
        VoteCount = crew.VoteCount;
        Job = crew.Job;
        EpisodeCount = crew.EpisodeCount;

        NumberOfItems = person?.Casts
            .Where(c => c.MovieId == crew.Id || c.TvId == crew.Id || c.SeasonId == crew.Id || c.EpisodeId == crew.Id)
            .Sum(c => (c.Movie != null && c.Movie.VideoFiles.Count != 0 ? 1 : 0) + (c.Tv?.NumberOfEpisodes ?? 0)) ?? 0;

        HasItem = person?.Casts.Any(c =>
            (c.MovieId == crew.Id || c.TvId == crew.Id || c.SeasonId == crew.Id || c.EpisodeId == crew.Id) &&
            (c.Movie?.VideoFiles.Count != 0 || c.Tv?.Episodes.Any(e => e.VideoFiles.Count != 0) != null)) == true;

        HaveItems = person?.Casts
            .Where(c => c.MovieId == crew.Id || c.TvId == crew.Id || c.SeasonId == crew.Id || c.EpisodeId == crew.Id)
            .Sum(c => (c.Movie is { VideoFiles.Count: > 0 } ? 1 : 0)
                      + (c.Tv != null ? c.Tv.Episodes.Count(e => e.VideoFiles.Count != 0) : 0)) ?? 0;
    }

    public KnownFor(TmdbPersonCredit crew, string type, Person? person)
    {
        int year = crew.ReleaseDate.ParseYear();
        if (year == 0) year = crew.FirstAirDate.ParseYear();
        Character = crew.Character;
        Title = crew.Title ?? crew.Name;
        Backdrop = crew.BackdropPath;
        MediaType = type;
        Type = type;
        Id = crew.Id;
        HasItem = false;
        Adult = crew.Adult;
        Popularity = crew.Popularity;
        Character = crew.Character;
        Job = crew.Job;
        Department = crew.Department;
        Year = year;
        OriginalLanguage = crew.OriginalLanguage;
        Overview = crew.Overview;
        Popularity = crew.Popularity;
        Poster = crew.PosterPath;
        VoteAverage = crew.VoteAverage;
        VoteCount = crew.VoteCount;
        Job = crew.Job;
        EpisodeCount = crew.EpisodeCount;

        HasItem = person?.Crews.Any(c =>
            (c.MovieId == crew.Id || c.TvId == crew.Id || c.SeasonId == crew.Id || c.EpisodeId == crew.Id) &&
            (c.Movie?.VideoFiles.Count != 0 || c.Tv?.Episodes.Any(e => e.VideoFiles.Count != 0) != null)) == true;

        NumberOfItems = person?.Crews
            .Where(c => c.MovieId == crew.Id || c.TvId == crew.Id || c.SeasonId == crew.Id || c.EpisodeId == crew.Id)
            .Sum(c => (c.Movie != null && c.Movie.VideoFiles.Count != 0 ? 1 : 0) + (c.Tv?.NumberOfEpisodes ?? 0)) ?? 0;

        HaveItems = person?.Crews
            .Where(c => c.MovieId == crew.Id || c.TvId == crew.Id || c.SeasonId == crew.Id || c.EpisodeId == crew.Id)
            .Sum(c => (c.Movie is { VideoFiles.Count: > 0 } ? 1 : 0)
                      + (c.Tv != null ? c.Tv.Episodes.Count(e => e.VideoFiles.Count != 0) : 0)) ?? 0;
    }
}

public record Images
{
    [JsonProperty("profiles")] public ImageDto[] Profiles { get; set; }
}

public record Profile
{
    [JsonProperty("aspect_ratio")] public double AspectRatio { get; set; }
    [JsonProperty("height")] public long Height { get; set; }
    [JsonProperty("iso_639_1")] public object Iso6391 { get; set; }
    [JsonProperty("file_path")] public string FilePath { get; set; }
    [JsonProperty("vote_average")] public double VoteAverage { get; set; }
    [JsonProperty("vote_count")] public long VoteCount { get; set; }
    [JsonProperty("width")] public long Width { get; set; }
}

public record TranslationsDto
{
    [JsonProperty("translations")] public TranslationDto[] TranslationsTranslations { get; set; }
}

public record TranslationDto
{
    public TranslationDto(Translation translation)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        EnglishName = translation.EnglishName;
        Name = translation.Name ?? string.Empty;
        Biography = translation.Biography ?? string.Empty;
    }

    public TranslationDto(TmdbCombinedTranslation translation)
    {
        Iso31661 = translation.Iso31661;
        Iso6391 = translation.Iso6391;
        EnglishName = translation.EnglishName;
        Name = translation.Data.Name ?? string.Empty;
        Biography = translation.Data.Biography ?? string.Empty;
    }

    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; }
    [JsonProperty("iso_639_1")] public string Iso6391 { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("english_name")] public string EnglishName { get; set; }
    [JsonProperty("biography")] public string Biography { get; set; }
}