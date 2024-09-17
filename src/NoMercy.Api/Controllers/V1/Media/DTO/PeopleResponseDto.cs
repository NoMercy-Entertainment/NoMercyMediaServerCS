using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.V1.Media.DTO;

public record PeopleResponseDto
{
    [JsonProperty("nextId")] public long? NextId { get; set; }
    [JsonProperty("data")] public IEnumerable<PeopleResponseItemDto> Data { get; set; } = [];

    public static async Task<List<PeopleResponseItemDto>> GetPeople(Guid userId, string language, int take,
        int page = 0)
    {
        await using MediaContext mediaContext = new();

        IIncludableQueryable<Person, IEnumerable<Translation>> query = mediaContext.People
            .AsNoTracking()
            .Where(person =>
                person.Casts
                    .Any(cast => cast.Tv != null && cast.Tv.Library.LibraryUsers
                        .FirstOrDefault(u => u.UserId == userId) != null) ||
                person.Casts.Any(cast => cast.Movie != null && cast.Movie.Library.LibraryUsers
                    .FirstOrDefault(u => u.UserId == userId) != null)
            )
            .Include(person => person.Translations
                .Where(translation => translation.Iso6391 == language));


        int totalItems = await query.CountAsync();
        Logger.Http($"Total items found: {totalItems}");

        IQueryable<Person> paginatedQuery = query
            .OrderByDescending(person => person.Popularity)
            .Skip(page * take)
            .Take(take);

        List<Person> people = await paginatedQuery.ToListAsync();

        Logger.Http($"Page: {page}, Take: {take}, Items returned: {people.Count}");

        return people
            .Select(person => new PeopleResponseItemDto(person))
            .ToList();
    }
}

public record PeopleResponseItemDto
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("adult")] public bool Adult { get; set; }
    [JsonProperty("also_known_as")] public string[]? AlsoKnownAs { get; set; }
    [JsonProperty("biography")] public string? Biography { get; set; }
    [JsonProperty("birthday")] public DateTime? Birthday { get; set; }
    [JsonProperty("color_palette")] public IColorPalettes? ColorPalette { get; set; }
    [JsonProperty("deathday")] public DateTime? DeathDay { get; set; }
    [JsonProperty("gender")] public string Gender { get; set; }
    [JsonProperty("homepage")] public string? Homepage { get; set; }
    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
    [JsonProperty("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonProperty("media_type")] public string MediaType { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("place_of_birth")] public string? PlaceOfBirth { get; set; }
    [JsonProperty("popularity")] public double Popularity { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("profile")] public string? Profile { get; set; }
    [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }

    public PeopleResponseItemDto(Person person)
    {
        string biography = person.Translations
            .FirstOrDefault()?.Biography ?? person.Biography ?? string.Empty;

        Id = person.Id;
        Name = person.Name;
        Biography = biography;
        Adult = person.Adult;
        AlsoKnownAs = person.AlsoKnownAs is null
            ? []
            : JsonConvert.DeserializeObject<string[]>(person.AlsoKnownAs);
        Birthday = person.BirthDay;
        DeathDay = person.DeathDay;
        Gender = person.Gender;
        Homepage = person.Homepage;
        ImdbId = person.ImdbId;
        KnownForDepartment = person.KnownForDepartment;
        PlaceOfBirth = person.PlaceOfBirth;
        Popularity = person.Popularity;
        Profile = person.Profile;
        Poster = person.Profile;
        ColorPalette = person.ColorPalette;
        CreatedAt = person.CreatedAt;
        UpdatedAt = person.UpdatedAt;
        MediaType = "person";
        Type = "person";
    }
}