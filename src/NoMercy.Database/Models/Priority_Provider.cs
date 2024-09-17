#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NoMercy.Database.Models;

[PrimaryKey(nameof(Priority), nameof(Country), nameof(ProviderId))]
[Index(nameof(Priority))]
[Index(nameof(Country))]
[Index(nameof(ProviderId))]
public class PriorityProvider
{
    [JsonProperty("priority")] public int Priority { get; set; }
    [JsonProperty("country")] public string Country { get; set; }

    [JsonProperty("provider_id")] public string ProviderId { get; set; }
    public Provider Provider { get; set; }

    public PriorityProvider()
    {
    }

    public PriorityProvider(int priority, string country, string providerId)
    {
        Priority = priority;
        Country = country;
        ProviderId = providerId;
    }
}