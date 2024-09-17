#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(TvId))]
[Index(nameof(SeasonId))]
public class Episode : ColorPalettes
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")] public string? Title { get; set; }
    [JsonProperty("air_date")] public DateTime? AirDate { get; set; }
    [JsonProperty("episode_number")] public int EpisodeNumber { get; set; }
    [JsonProperty("imdb_id")] public string? ImdbId { get; set; }
    [JsonProperty("overview")] public string? Overview { get; set; }
    [JsonProperty("production_code")] public string? ProductionCode { get; set; }
    [JsonProperty("season_number")] public int SeasonNumber { get; set; }
    [JsonProperty("still")] public string? Still { get; set; }
    [JsonProperty("tvdb_id")] public int? TvdbId { get; set; }
    [JsonProperty("vote_average")] public float? VoteAverage { get; set; }
    [JsonProperty("vote_count")] public int? VoteCount { get; set; }

    [JsonProperty("tv_id")] public int TvId { get; set; }
    public Tv Tv { get; set; }

    [JsonProperty("season_id")] public int SeasonId { get; set; }
    public Season Season { get; set; }

    [JsonProperty("casts")] public ICollection<Cast> Cast { get; set; }
    [JsonProperty("crews")] public ICollection<Crew> Crew { get; set; }
    [JsonProperty("special_items")] public ICollection<SpecialItem> SpecialItems { get; set; }

    [JsonProperty("video_files")] public ICollection<VideoFile> VideoFiles { get; set; } = new HashSet<VideoFile>();

    [JsonProperty("medias")] public ICollection<Media> Media { get; set; }
    [JsonProperty("images")] public ICollection<Image> Images { get; set; }
    [JsonProperty("guest_stars")] public ICollection<GuestStar> GuestStars { get; set; }
    [JsonProperty("files")] public ICollection<File> Files { get; set; }
    [JsonProperty("translations")] public ICollection<Translation> Translations { get; set; }

    public Episode()
    {
    }

    // public Episode(TmdbEpisodeAppends e, int tvId, int seasonId)
    // {
    //     Id = e.Id;
    //     Title = e.Name;
    //     AirDate = e.AirDate;
    //     EpisodeNumber = e.EpisodeNumber;
    //     ImdbId = e.TmdbEpisodeExternalIds.ImdbId;
    //     Overview = e.Overview;
    //     ProductionCode = e.ProductionCode;
    //     SeasonNumber = e.SeasonNumber;
    //     Still = e.StillPath;
    //     TvdbId = e.TmdbEpisodeExternalIds.TvdbId;
    //     VoteAverage = e.VoteAverage;
    //     VoteCount = e.VoteCount;
    //
    //     TvId = tvId;
    //     SeasonId = seasonId;
    // }
}