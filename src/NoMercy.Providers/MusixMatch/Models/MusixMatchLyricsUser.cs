#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchLyricsUser
{
    [JsonProperty("user")] public MusixMatchUser MusixMatchUser;
}