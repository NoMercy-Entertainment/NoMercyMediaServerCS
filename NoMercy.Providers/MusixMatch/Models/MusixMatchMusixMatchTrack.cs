#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;
using NoMercy.NmSystem;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchMusixMatchTrack
{
    [JsonProperty("track_id")] public long TrackId { get; set; }
    [JsonProperty("track_mbid")] public string TrackMbid { get; set; }
    [JsonProperty("track_isrc")] public string TrackIsrc { get; set; }
    [JsonProperty("commontrack_isrcs")] public string[][] CommontrackIsrcs { get; set; }
    [JsonProperty("track_spotify_id")] public string TrackSpotifyId { get; set; }

    [JsonProperty("commontrack_spotify_ids")]
    public string[] CommontrackSpotifyIds { get; set; }

    [JsonProperty("commontrack_itunes_ids")]
    public long[] CommontrackItunesIds { get; set; }

    [JsonProperty("track_soundcloud_id")] public long TrackSoundcloudId { get; set; }
    [JsonProperty("track_xboxmusic_id")] public string TrackXboxmusicId { get; set; }
    [JsonProperty("track_name")] public string TrackName { get; set; }

    [JsonProperty("track_name_translation_list")]
    public object[] TrackNameTranslationList { get; set; }

    [JsonProperty("track_rating")] public long TrackRating { get; set; }
    [JsonProperty("track_length")] public long TrackLength { get; set; }
    [JsonProperty("commontrack_id")] public long CommontrackId { get; set; }
    [JsonProperty("instrumental")] public long Instrumental { get; set; }
    [JsonProperty("explicit")] public long Explicit { get; set; }
    [JsonProperty("has_lyrics")] public long HasLyrics { get; set; }
    [JsonProperty("has_lyrics_crowd")] public long HasLyricsCrowd { get; set; }
    [JsonProperty("has_subtitles")] public long HasSubtitles { get; set; }
    [JsonProperty("has_richsync")] public long HasRichsync { get; set; }
    [JsonProperty("has_track_structure")] public long HasTrackStructure { get; set; }
    [JsonProperty("num_favourite")] public long NumFavourite { get; set; }
    [JsonProperty("lyrics_id")] public long LyricsId { get; set; }
    [JsonProperty("subtitle_id")] public long SubtitleId { get; set; }
    [JsonProperty("album_id")] public long AlbumId { get; set; }
    [JsonProperty("album_name")] public long AlbumName { get; set; }
    [JsonProperty("album_vanity_id")] public string AlbumVanityId { get; set; }
    [JsonProperty("artist_id")] public long ArtistId { get; set; }
    [JsonProperty("artist_mbid")] public Guid ArtistMbid { get; set; }
    [JsonProperty("artist_name")] public string ArtistName { get; set; }

    [JsonProperty("album_coverart_100x100")]
    public Uri AlbumCoverart100X100 { get; set; }

    [JsonProperty("album_coverart_350x350")]
    public Uri AlbumCoverart350X350 { get; set; }

    [JsonProperty("album_coverart_500x500")]
    public Uri AlbumCoverart500X500 { get; set; }

    [JsonProperty("album_coverart_800x800")]
    public Uri AlbumCoverart800X800 { get; set; }

    [JsonProperty("track_share_url")] public Uri TrackShareUrl { get; set; }
    [JsonProperty("track_edit_url")] public Uri TrackEditUrl { get; set; }

    [JsonProperty("commontrack_vanity_id")]
    public string CommontrackVanityId { get; set; }

    [JsonProperty("restricted")] public long Restricted { get; set; }

    [JsonProperty("first-release-date")] private string? _firstReleaseDate { get; set; }

    public DateTime? FirstReleaseDate
    {
        get => DateTimeParser.ParseDateTime(_firstReleaseDate);
        set => _firstReleaseDate = value.ToString();
    }

    [JsonProperty("updated_time")] public DateTimeOffset UpdatedTime { get; set; }
    [JsonProperty("primary_genres")] public MusixMatchGenres PrimaryMusixMatchGenres { get; set; }
    [JsonProperty("secondary_genres")] public MusixMatchGenres SecondaryMusixMatchGenres { get; set; }
}