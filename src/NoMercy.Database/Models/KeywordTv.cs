#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(KeywordId), nameof(TvId))]
[Index(nameof(KeywordId))]
[Index(nameof(TvId))]
public class KeywordTv
{
    [JsonProperty("keyword_id")] public int KeywordId { get; set; }
    public Keyword Keyword { get; set; }

    [JsonProperty("tv_id")] public int TvId { get; set; }
    public Tv Tv { get; set; }

    public KeywordTv()
    {
    }
}