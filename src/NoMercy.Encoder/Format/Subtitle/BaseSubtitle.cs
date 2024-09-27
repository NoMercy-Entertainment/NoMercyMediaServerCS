using FFMpegCore;
using NoMercy.Encoder.Core;
using NoMercy.Encoder.Format.Rules;
using NoMercy.NmSystem;
using Serilog.Events;

namespace NoMercy.Encoder.Format.Subtitle;

public class BaseSubtitle : Classes
{
    #region Properties

    public CodecDto SubtitleCodec { get; set; } = SubtitleCodecs.Webvtt;

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

    internal string HlsSegmentFilename
    {
        get => _hlsSegmentFilename
            .Replace(":language:", Language)
            .Replace(":codec:", SubtitleCodec.SimpleValue)
            .Replace(":type:", Type)
            .Replace("/", Str.DirectorySeparator)
            .Replace("\\", Str.DirectorySeparator);
        set => _hlsSegmentFilename = value;
    }

    private string _hlsPlaylistFilename = "";

    internal string HlsPlaylistFilename
    {
        get => _hlsPlaylistFilename
            .Replace(":language:", Language)
            .Replace(":codec:", SubtitleCodec.SimpleValue)
            .Replace(":type:", Type)
            .Replace(":variant:", "full")
            .Replace("/", Str.DirectorySeparator)
            .Replace("\\", Str.DirectorySeparator);
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
        // AddCustomArgument("-map_metadata", -1);
        // AddCustomArgument("-fflags", "+bitexact");
        // AddCustomArgument("-flags:v", "+bitexact");
        // AddCustomArgument("-flags:a", "+bitexact");
        // AddCustomArgument("-flags:s", "+bitexact");
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

            newStream.Index = SubtitleStreams.IndexOf(newStream.SubtitleStream);
            
            newStream.Extension = GetExtension(newStream);

            streams.Add(newStream);
        }

        return streams;
    }

    private string GetExtension(BaseSubtitle stream)
    {
        string ext = "vtt";
            
        if (stream.SubtitleStream!.CodecName == "hdmv_pgs_subtitle" || stream.SubtitleStream.CodecName == "dvd_subtitle")
        {
            stream.SubtitleCodec = SubtitleCodecs.Copy;
            stream.ConvertSubtitle = true;
            ext = "sup";
        }

        return ext;
    }

    public void AddToDictionary(Dictionary<string, dynamic> commandDictionary, int index)
    {
        // commandDictionary["-map"] = $"[s{index}_hls_0]";
        commandDictionary["-map"] = $"s:{index}";
        commandDictionary["-c:s"] = SubtitleCodec.Value;
        
        if (!IsoLanguageMapper.IsoToLanguage.TryGetValue(Language, out string? language))
        {
            throw new Exception($"Language {Language} is not supported");
        }
        commandDictionary[$"-metadata:s:s:{index}"] = $"title=\"{language}\"";
        commandDictionary[$"-metadata:s:s:{index}"] = $"language=\"{Language}\"";

        foreach (KeyValuePair<string, dynamic> extraParameter in _extraParameters)
        {
            commandDictionary[extraParameter.Key] = extraParameter.Value;
        }
    }

    public void CreateFolder()
    {
        string path = Path.Combine(BasePath, HlsPlaylistFilename.Split(Str.DirectorySeparator).First());
        Logger.Encoder($"Creating folder {path}", LogEventLevel.Verbose);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    #endregion

    public static BaseSubtitle Create(string profileCodec)
    {
        return profileCodec switch
        {
            "vtt" => new Vtt(),
            "srt" => new Srt(),
            "_" => throw new Exception($"Subtitle {profileCodec} is not supported"),
        };
    }
}