#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Newtonsoft.Json;

namespace NoMercy.Providers.MusixMatch.Models;

public class MusixMatchUserBlobGet
{
    [JsonProperty("message")] public UserBlobGetMessage Message { get; set; }
    [JsonProperty("meta")] public UserBlobGetMeta Meta { get; set; }
}