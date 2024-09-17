#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class MusicGenre
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    public MusicGenre()
    {
    }

    // public MusicGenre(Providers.MusicBrainz.Models.MusicBrainzGenre musicBrainzGenre)
    // {
    //     Id = musicBrainzGenre.Id;
    //     Name = musicBrainzGenre.Name;
    // }
}