using Newtonsoft.Json;
using NoMercy.Api.Controllers.V1.Media.DTO;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Providers.TMDB.Models.Movies;
using NoMercy.Providers.TMDB.Models.Shared;
using NoMercy.Providers.TMDB.Models.TV;
using TmdbGender = NoMercy.Providers.TMDB.Models.People.TmdbGender;

namespace NoMercy.Api.Controllers.V1.DTO;

public record VideoDto
{
    [JsonProperty("src")] public string? Src { get; set; }
    [JsonProperty("type")] public string? Type { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("site")] public string? Site { get; set; }
    [JsonProperty("size")] public int Size { get; set; }

    public VideoDto(Database.Models.Media media)
    {
        Src = media.Src;
        Type = media.Type;
        Name = media.Name;
        Site = media.Site;
        Size = media.Size;
    }

    public VideoDto(TmdbTvVideo media)
    {
        Src = media.Key;
        Type = "video";
        Name = media.Name;
        Site = media.Site;
        Size = media.Size;
    }

    public VideoDto(TmdbMovieVideo media)
    {
        Src = media.Key;
        Type = "video";
        Name = media.Name;
        Site = media.Site;
        Size = media.Size;
    }
}

public record GenreDto
{
    [JsonProperty("id")] public dynamic Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }

    public GenreDto(GenreMovie genreMovie)
    {
        Id = genreMovie.GenreId;
        Name = genreMovie.Genre.Name;
    }

    public GenreDto(GenreTv genreTv)
    {
        Id = genreTv.GenreId;
        Name = genreTv.Genre.Name;
    }

    public GenreDto(Genre genreMovie)
    {
        Id = genreMovie.Id;
        Name = genreMovie.Name;
    }

    public GenreDto(TmdbGenre tmdbGenreMovie)
    {
        Id = tmdbGenreMovie.Id;
        Name = tmdbGenreMovie.Name;
    }

    public GenreDto(ArtistMusicGenre artistMusicGenre)
    {
        Id = artistMusicGenre.MusicGenreId;
        Name = artistMusicGenre.MusicGenre.Name;
    }

    public GenreDto(AlbumMusicGenre artistMusicGenre)
    {
        Id = artistMusicGenre.MusicGenreId;
        Name = artistMusicGenre.MusicGenre.Name;
    }
}

public record RatingDto
{
    [JsonProperty("rating")] public string RatingRating { get; set; } = string.Empty;
    [JsonProperty("iso_3166_1")] public string Iso31661 { get; set; } = string.Empty;
}

public record ColorPalettesDto
{
    [JsonProperty("logo")] public IColorPalettes Logo { get; set; } = new();
    [JsonProperty("poster")] public IColorPalettes Poster { get; set; } = new();
    [JsonProperty("backdrop")] public IColorPalettes Backdrop { get; set; } = new();
}

public record StatusResponseDto<T>
{
    [JsonProperty("status")] public string Status { get; set; } = "ok";
    [JsonProperty("data")] public T Data { get; set; } = default!;
    [JsonProperty("message")] public string? Message { get; set; } = "NoMercy is running!";
    [JsonProperty("args")] public dynamic[]? Args { get; set; } = [];
}

public record AvailableResponseDto
{
    [JsonProperty("available")] public bool Available { get; set; }
    [JsonProperty("server")] public string? Message { get; set; }
}

public record LikeRequestDto
{
    [JsonProperty("value")] public bool Value { get; set; }
}

public record PeopleDto
{
    [JsonProperty("character")] public string? Character { get; set; }
    [JsonProperty("job")] public string? Job { get; set; }
    [JsonProperty("profile")] public string? ProfilePath { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("deathday")] public DateTime? DeathDay { get; set; }
    [JsonProperty("translations")] public TranslationDto[] Translations { get; set; }
    [JsonProperty("order")] public int? Order { get; set; }

    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }

    public PeopleDto(Cast cast)
    {
        Id = cast.Person.Id;
        Character = cast.Role.Character;
        ProfilePath = cast.Person.Profile;
        KnownForDepartment = cast.Person.KnownForDepartment;
        Name = cast.Person.Name;
        ColorPalette = cast.Person.ColorPalette;
        DeathDay = cast.Person.DeathDay;
        Gender = cast.Person.Gender;
        Order = cast.Role.Order;
        Translations = [];
    }

    public PeopleDto(TmdbCast tmdbCast)
    {
        Id = tmdbCast.Id;
        Character = tmdbCast.Character;
        ProfilePath = tmdbCast.ProfilePath;
        KnownForDepartment = tmdbCast.KnownForDepartment;
        Name = tmdbCast.Name ?? string.Empty;
        ColorPalette = new IColorPalettes();
        Gender = Enum.Parse<TmdbGender>(tmdbCast.Gender.ToString(), true).ToString();
        Order = tmdbCast.Order;
        Translations = [];
    }

    public PeopleDto(Crew crew)
    {
        Id = crew.Person.Id;
        Job = crew.Job.Task;
        ProfilePath = crew.Person.Profile;
        KnownForDepartment = crew.Person.KnownForDepartment;
        Name = crew.Person.Name;
        ColorPalette = crew.Person.ColorPalette;
        DeathDay = crew.Person.DeathDay;
        Gender = crew.Person.Gender;
        Order = crew.Job.Order;
        Translations = [];
    }

    public PeopleDto(TmdbCrew tmdbCrew)
    {
        Id = tmdbCrew.Id;
        Job = tmdbCrew.Job;
        ProfilePath = tmdbCrew.ProfilePath;
        KnownForDepartment = tmdbCrew.KnownForDepartment;
        Name = tmdbCrew.Name;
        ColorPalette = new IColorPalettes();
        Gender = Enum.Parse<TmdbGender>(tmdbCrew.Gender.ToString(), true).ToString();
        Order = tmdbCrew.Order;
        Translations = [];
    }

    public PeopleDto(TmdbCreatedBy crew)
    {
        Id = crew.Id;
        Job = "Creator";
        ProfilePath = crew.ProfilePath;
        Name = crew.Name;
        ColorPalette = new IColorPalettes();
        Gender = Enum.Parse<TmdbGender>(crew.Gender.ToString(), true).ToString();
        Translations = [];
    }

    public PeopleDto(Creator creator)
    {
        Id = creator.Person.Id;
        Job = "Creator";
        ProfilePath = creator.Person.Profile;
        KnownForDepartment = creator.Person.KnownForDepartment;
        Name = creator.Person.Name;
        ColorPalette = creator.Person.ColorPalette;
        DeathDay = creator.Person.DeathDay;
        Gender = creator.Person.Gender;
        Translations = [];
    }
}

public record ContentRating
{
    [JsonProperty("rating")] public string? Rating { get; set; }
    [JsonProperty("iso_3166_1")] public string? Iso31661 { get; set; }
}

public record DataResponseDto<T>
{
    [JsonProperty("data")] public T? Data { get; set; }
}

public record PaginatedResponse<T>
{
    [JsonProperty("data")] public IEnumerable<T> Data { get; set; } = [];
    [JsonProperty("next_page")] public int? NextPage { get; set; }
    [JsonProperty("has_more")] public bool HasMore { get; set; }
}