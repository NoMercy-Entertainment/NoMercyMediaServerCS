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