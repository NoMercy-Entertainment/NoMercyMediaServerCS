using Microsoft.AspNetCore.Mvc;

namespace NoMercy.Api.Controllers.V1.Media;
public class PageRequestDto
{
    [FromQuery(Name = "page")] public int Page { get; set; } = 1;
    [FromQuery(Name = "take")] public int Take { get; set; } = 10;
    [FromQuery(Name = "version")] public string? Version { get; set; }
}