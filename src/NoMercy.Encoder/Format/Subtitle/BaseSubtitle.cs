using FFMpegCore;
using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Subtitle;

public class BaseSubtitle : Classes
{
    #region Properties

    public CodecDto SubtitleCodec { get; set; } = new()
    {
        Name = "ASS",
        Value = "ass",
        IsDefault = false
    };

    protected internal SubtitleStream? SubtitleStream;
    internal List<SubtitleStream> SubtitleStreams { get; set; } = [];

    public string Language => SubtitleStream?.Language ?? "und";
    private string[] AllowedLanguages { get; set; } = ["eng"];
    public int StreamIndex => SubtitleStream?.Index ?? -1;

    private readonly Dictionary<string, dynamic> _extraParameters = [];
    private readonly Dictionary<string, dynamic> _filters = [];
    private readonly Dictionary<string, dynamic> _ops = [];
    protected virtual CodecDto[] AvailableCodecs => [];
    protected virtual string[] AvailableContainers => [];

    private string _hlsSegmentFilename = "";

    private string HlsSegmentFilename
    {
        get => _hlsSegmentFilename
            .Replace(":language:", Language)
            .Replace(":codec:", SubtitleCodec.SimpleValue)
            .Replace(":type:", Type);
        set => _hlsSegmentFilename = value;
    }

    private string _hlsPlaylistFilename = "";

    internal string HlsPlaylistFilename
    {
        get => _hlsPlaylistFilename
            .Replace(":language:", Language)
            .Replace(":codec:", SubtitleCodec.SimpleValue)
            .Replace(":type:", Type);
        set => _hlsPlaylistFilename = value;
    }

    public dynamic Data => new
    {
        SubtitleCodec,
        Type,
        _extraParameters,
        _ops,
        _filters
    };

    #endregion

    #region Setters

    protected BaseSubtitle SetSubtitleCodec(string subtitleCodec)
    {
        CodecDto[] availableCodecs = AvailableCodecs;
        if (availableCodecs.All(codec => codec.Value != subtitleCodec))
            throw new Exception(
                $"Wrong subtitle codec value for {subtitleCodec}, available formats are {string.Join(", ", AvailableCodecs.Select(codec => codec.Value))}");

        SubtitleCodec = availableCodecs.First(codec => codec.Value == subtitleCodec);

        return this;
    }

    protected BaseSubtitle AddCustomArgument(string key, dynamic i)
    {
        _extraParameters.Add(key, i);
        return this;
    }

    public BaseSubtitle AddOpts(string key, dynamic value)
    {
        _ops.Add(key, value);
        return this;
    }

    public BaseSubtitle SetHlsSegmentFilename(string value)
    {
        HlsSegmentFilename = value;
        return this;
    }

    public BaseSubtitle SetHlsPlaylistFilename(string value)
    {
        HlsPlaylistFilename = value;
        return this;
    }

    public BaseSubtitle SetAllowedLanguages(string[] languages)
    {
        AllowedLanguages = languages;
        return this;
    }

    public override BaseSubtitle ApplyFlags()
    {
        AddCustomArgument("-map_metadata", -1);
        AddCustomArgument("-fflags", "+bitexact");
        AddCustomArgument("-flags:v", "+bitexact");
        AddCustomArgument("-flags:a", "+bitexact");
        AddCustomArgument("-flags:s", "+bitexact");
        return this;
    }

    public List<BaseSubtitle> Build()
    {
        List<BaseSubtitle> streams = [];

        foreach (string allowedLanguage in AllowedLanguages)
        {
            if (SubtitleStreams.All(stream => stream.Language != allowedLanguage)) continue;

            BaseSubtitle newStream = (BaseSubtitle)MemberwiseClone();

            newStream.IsSubtitle = true;

            newStream.SubtitleStream = SubtitleStreams
                .Find(stream => stream.Language == allowedLanguage)!;

            newStream.Index = newStream.SubtitleStream.Index - 1;

            streams.Add(newStream);
        }

        return streams;
    }

    public void AddToDictionary(Dictionary<string, dynamic> commandDictionary, int index)
    {
        commandDictionary["-map"] = $"[s{index}_hls_0]";
        commandDictionary["-c:s"] = SubtitleCodec.Value;

        commandDictionary[$"-metadata:s:s:{index}"] = $"language=\"{Language}\"";
        commandDictionary[$"-metadata:s:s:{index}"] = $"title=\"{Language} {SubtitleCodec.SimpleValue}\"";

        foreach (KeyValuePair<string, dynamic> extraParameter in _extraParameters)
            commandDictionary[extraParameter.Key] = extraParameter.Value;
    }

    public void CreateFolder()
    {
        string path = Path.Combine(BasePath, HlsSegmentFilename.Split("/").First());
        // Logger.Encoder($"Creating folder {path}");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    #endregion
}