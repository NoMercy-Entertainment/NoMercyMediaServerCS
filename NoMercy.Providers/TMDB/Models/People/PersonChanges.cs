﻿using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.People;

public class PersonChanges
{
    [JsonProperty("changes")] public Change[] Changes { get; set; } = [];
}

public class Change
{
    [JsonProperty("key")] public string Key { get; set; } = string.Empty;

    [JsonProperty("items")] public Tem[] Items { get; set; } = [];
}

public class Tem
{
    [JsonProperty("id")] public string Id { get; set; } = string.Empty;

    [JsonProperty("action")] public string Action { get; set; } = string.Empty;

    [JsonProperty("time")] public string Time { get; set; } = string.Empty;

    [JsonProperty("original_value")] public OriginalValue OriginalValue { get; set; } = new();
}

public class OriginalValue
{
    [JsonProperty("profile")] public string Profile { get; set; } = string.Empty;
}