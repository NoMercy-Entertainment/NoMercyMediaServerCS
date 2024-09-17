#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchTrackSearchParameters
{
    public static Dictionary<MusixMatchSortStrategy, KeyValuePair<string, string>> StrategyDecryptions = new()
    {
        [MusixMatchSortStrategy.TrackRatingAsc] = new KeyValuePair<string, string>("s_track_rating", "asc"),
        [MusixMatchSortStrategy.TrackRatingDesc] = new KeyValuePair<string, string>("s_track_rating", "desc"),
        [MusixMatchSortStrategy.ArtistRatingAsc] = new KeyValuePair<string, string>("s_artist_rating", "asc"),
        [MusixMatchSortStrategy.ArtistRatingDesc] = new KeyValuePair<string, string>("s_artist_rating", "desc"),
        [MusixMatchSortStrategy.ReleaseDateAsc] = new KeyValuePair<string, string>("s_track_release_date", "asc"),
        [MusixMatchSortStrategy.ReleaseDateDesc] = new KeyValuePair<string, string>("s_track_release_date", "desc")
    };

    public string? Query { get; set; } = "";
    public string? LyricsQuery { get; set; } = "";
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
    public string[]? Artists { get; set; }
    public string Album { get; set; } = "";
    public string? Duration { get; set; } = "";
    public string? Language { get; set; } = "";
    public bool? HasLyrics { get; set; } = true;
    public bool? HasSubtitles { get; set; }
    public bool? HasRichSync { get; set; }
    public MusixMatchSortStrategy? Sort { get; set; } = MusixMatchSortStrategy.TrackRatingDesc;


    public enum MusixMatchSortStrategy
    {
        TrackRatingAsc,
        TrackRatingDesc,
        ArtistRatingAsc,
        ArtistRatingDesc,
        ReleaseDateAsc,
        ReleaseDateDesc
    }
}