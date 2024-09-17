#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class Special : ColorPaletteTimeStamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public required Ulid Id { get; set; } = Ulid.NewUlid();

    [JsonProperty("backdrop")] public string? Backdrop { get; set; }
    [JsonProperty("description")] public string? Description { get; set; }
    [JsonProperty("poster")] public string? Poster { get; set; }
    [JsonProperty("logo")] public string? Logo { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("titleSort")] public string? TitleSort { get; set; }
    [JsonProperty("creator")] public string? Creator { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }

    [JsonProperty("items")] public ICollection<SpecialItem> Items { get; set; } = new HashSet<SpecialItem>();

    [JsonProperty("special_user")] public ICollection<SpecialUser> SpecialUser { get; set; }
}