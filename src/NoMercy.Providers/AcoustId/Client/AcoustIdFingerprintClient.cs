#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System.Diagnostics;
using AcoustID;
using Newtonsoft.Json;
using NoMercy.Networking;
using NoMercy.NmSystem;
using NoMercy.Providers.AcoustId.Models;

namespace NoMercy.Providers.AcoustId.Client;

public class AcoustIdFingerprintClient : AcoustIdBaseClient
{
    public AcoustIdFingerprintClient()
    {
        Configuration.ClientKey = ApiInfo.AcousticIdKey;
    }

    private Task<AcoustIdFingerprint?> WithFingerprint(string[] appendices, FingerPrintData fingerprintData,
        bool? priority = false)
    {
        Dictionary<string, string?> queryParams = new()
        {
            ["client"] = ApiInfo.AcousticIdKey,
            ["duration"] = fingerprintData.Duration.ToString(),
            ["fingerprint"] = fingerprintData.Fingerprint
        };

        return Get<AcoustIdFingerprint>("lookup?meta=" + string.Join("+", appendices), queryParams, priority);
    }

    public ValueTask<AcoustIdFingerprint?> Lookup(string? file, bool? priority = false)
    {
        if (file == null) return ValueTask.FromResult<AcoustIdFingerprint?>(null);

        Process process = new();
        process.StartInfo.FileName = AppFiles.FpCalcPath;
        process.StartInfo.Arguments = "-json \"" + file + "\"";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        FingerPrintData? fingerprintData = output.FromJson<FingerPrintData>();

        if (fingerprintData == null) throw new Exception("Fingerprint data is null");

        return new ValueTask<AcoustIdFingerprint?>(WithFingerprint([
            "recordings",
            "releases",
            "tracks",
            "compress",
            "usermeta",
            "sources"
        ], fingerprintData, priority));
    }
}

public class FingerPrintData
{
    [JsonProperty("duration")] public double _duration { get; set; }
    [JsonProperty("fingerprint")] public string Fingerprint { get; set; }

    public int Duration
    {
        get => (int)Math.Floor(_duration);
        set => _duration = value;
    }
}