﻿using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Networks;

public class NetworkAlternativeNames
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("results")] public Result[] Results { get; set; } = [];
}

public class Result
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;

    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}