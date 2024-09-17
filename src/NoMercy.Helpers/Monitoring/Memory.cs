using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace NoMercy.Helpers.Monitoring;
public class Memory
{
    [JsonProperty("available")] public double Available { get; set; }
    [JsonProperty("use")] public double Use { get; set; }
    [JsonProperty("total")] public double Total { get; set; }

    [JsonProperty("percentage")]
    public double Percentage =>
        Use / Total * (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 0.1 : 100);
}