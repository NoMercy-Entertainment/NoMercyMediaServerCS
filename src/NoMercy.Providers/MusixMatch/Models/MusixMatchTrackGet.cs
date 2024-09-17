#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchTrackGet
{
    [JsonProperty("track")] public MusixMatchMusixMatchTrack MusixMatchMusixMatchTrack;
}