#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;

namespace NoMercy.Api.Controllers.V1.Media.DTO;
public record SpecialsResponseDto
{
    [JsonProperty("nextId")] public object NextId { get; set; }

    [JsonProperty("data")] public IOrderedEnumerable<SpecialsResponseItemDto> Data { get; set; }

    public static readonly Func<MediaContext, Guid, string, IAsyncEnumerable<SpecialsResponseItemDto>> GetSpecials =
        EF.CompileAsyncQuery((MediaContext mediaContext, Guid userId, string language) =>
            mediaContext.Specials.AsNoTracking()
                .Include(special => special.Items)
                .ThenInclude(item => item.Episode)
                .ThenInclude(episode => episode!.Tv)
                .Include(special => special.Items)
                .ThenInclude(item => item.Episode)
                .ThenInclude(episode => episode!.VideoFiles)
                .Include(special => special.Items)
                .ThenInclude(item => item.Movie)
                .ThenInclude(movie => movie!.VideoFiles)
                .Select(special => new SpecialsResponseItemDto(special))
        );
}