#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(CertificationId), nameof(TvId))]
[Index(nameof(CertificationId))]
[Index(nameof(TvId))]
public class CertificationTv
{
    [JsonProperty("certification_id")] public int CertificationId { get; set; }
    public Certification Certification { get; set; }

    [JsonProperty("tv_id")] public int TvId { get; set; }
    public Tv Tv { get; set; }

    public CertificationTv()
    {
    }
}