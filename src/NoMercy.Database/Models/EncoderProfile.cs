#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Id))]
public class EncoderProfile : Timestamps
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [JsonProperty("id")]
    public Ulid Id { get; set; }

    [Key] [JsonProperty("name")] public required string Name { get; set; }
    [JsonProperty("container")] public string? Container { get; set; }
    [JsonProperty("param")] public string? Param { get; set; }


    [Column("VideoProfile")]
    [JsonProperty("video_profile")]
    [JsonIgnore]
    public string _videoProfiles { get; set; } = string.Empty;

    [NotMapped]
    public IVideoProfile[] VideoProfiles
    {
        get => _videoProfiles != string.Empty
            ? JsonConvert.DeserializeObject<IVideoProfile[]>(_videoProfiles)!
            : [];
        set => _videoProfiles = JsonConvert.SerializeObject(value);
    }

    [Column("AudioProfile")]
    [JsonProperty("audio_profile")]
    [JsonIgnore]
    public string _audioProfiles { get; set; } = string.Empty;

    [NotMapped]
    public IAudioProfile[] AudioProfiles
    {
        get => _audioProfiles != string.Empty
            ? JsonConvert.DeserializeObject<IAudioProfile[]>(_audioProfiles)!
            : [];
        set => _audioProfiles = JsonConvert.SerializeObject(value);
    }

    [Column("SubtitleProfile")]
    [JsonProperty("subtitle_profile")]
    [JsonIgnore]
    public string _subtitleProfiles { get; set; } = string.Empty;

    [NotMapped]
    public ISubtitleProfile[] SubtitleProfiles
    {
        get => _subtitleProfiles != string.Empty
            ? JsonConvert.DeserializeObject<ISubtitleProfile[]>(_subtitleProfiles)!
            : [];
        set => _subtitleProfiles = JsonConvert.SerializeObject(value);
    }








    [JsonProperty("encoder_profile_folder")]
    public ICollection<EncoderProfileFolder> EncoderProfileFolder { get; set; }
}

public class IVideoProfile
{
    public string Codec { get; set; }
    public int Bitrate { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Framerate { get; set; }
    public string Preset { get; set; }
    public string Tune { get; set; }
    public string SegmentName { get; set; }
    public string PlaylistName { get; set; }
    public string ColorSpace { get; set; }
    public int Crf { get; set; }
    public int Keyint { get; set; }
    public string[] Opts { get; set; }
    public (string key, string Val)[] CustomArguments { get; set; }
}
public class IAudioProfile
{
    public string Codec { get; set; }
    public int Channels { get; set; }
    public string SegmentName { get; set; }
    public string PlaylistName { get; set; }
    public string[] AllowedLanguages { get; set; }
}

public class ISubtitleProfile
{
    public string Codec { get; set; }
    public string SegmentName { get; set; }
    public string PlaylistName { get; set; }
    public string[] AllowedLanguages { get; set; }
}