#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(CertificationId), nameof(MovieId))]
[Index(nameof(CertificationId))]
[Index(nameof(MovieId))]
public class CertificationMovie
{
    [JsonProperty("certification_id")] public int CertificationId { get; set; }
    public Certification Certification { get; set; }

    [JsonProperty("movie_id")] public int MovieId { get; set; }
    public Movie Movie { get; set; }

    public CertificationMovie()
    {
    }
}