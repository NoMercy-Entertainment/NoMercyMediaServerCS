using FFMpegCore;
using NoMercy.Encoder.Format.Rules;
using NoMercy.NmSystem;

namespace NoMercy.Encoder.Format.Video;

public abstract class BaseVideo : Classes
{
    #region Properties

    public virtual CodecDto VideoCodec { get; set; } = VideoCodecs.H264;

    protected internal VideoStream? VideoStream;

    internal List<VideoStream> VideoStreams { get; set; }

    protected internal virtual bool BFramesSupport => false;

    protected internal virtual int Modulus { get; set; }
    protected internal long Bitrate { get; set; }
    internal int BufferSize { get; set; }
    internal string Tune { get; set; }
    internal string Profile { get; set; }
    internal string Preset { get; set; }
    internal string PixelFormat { get; set; }
    internal int ConstantRateFactor { get; set; }
    internal int FrameRate { get; set; }
    internal int KeyIntMin { get; set; }
    internal int OutputWidth { get; set; }
    internal int? OutputHeight { get; set; }
    public int StreamIndex { get; set; } = 0;
    internal int MaxRate { get; set; }
    public bool HdrAllowed { get; set; }
    public bool ConverToSdr { get; set; }

    protected internal virtual string[] AvailableContainers => [];
    protected internal virtual string[] AvailablePresets => [];
    protected internal virtual string[] AvailableProfiles => [];
    protected internal virtual string[] AvailableTune => [];
    protected internal virtual string[] AvailableLevels => [];
    protected virtual CodecDto[] AvailableCodecs => [];

    internal readonly Dictionary<string, dynamic> _extraParameters = [];
    internal readonly Dictionary<string, dynamic> _filters = [];
    internal readonly Dictionary<string, dynamic> _ops = [];

    protected internal static VideoQualityDto[] AvailableVideoSizes =>
    [
        FrameSizes._240p, FrameSizes._360p,
        FrameSizes._480p, FrameSizes._720p,
        FrameSizes._1080p, FrameSizes._1440p,
        FrameSizes._4k, FrameSizes._8k
    ];

    internal string CropValue { get; set; } = "";

    protected internal CropArea Crop
    {
        get
        {
            if (string.IsNullOrEmpty(CropValue)) return new CropArea();
            int[] parts = CropValue.Split(':')
                .Select(int.Parse)
                .ToArray();
            return new CropArea(parts[0], parts[1], parts[2], parts[3]);
        }
        set => CropValue = $"crop={value.W}:{value.H}:{value.X}:{value.Y}";
    }

    internal double AspectRatioValue => Crop.H / Crop.W;

    internal string ScaleValue = "";

    public ScaleArea Scale
    {
        get
        {
            if (string.IsNullOrEmpty(ScaleValue))
                return new ScaleArea { W = 0, H = 0 };
            string[] scale = ScaleValue.Split(':');
            return new ScaleArea
            {
                W = scale[0].ToInt(),
                H = int.IsNegative(scale[1].ToInt())
                    ? Convert.ToInt32(scale[0].ToInt() * AspectRatioValue)
                    : scale[1].ToInt()
            };
        }
        set => ScaleValue = $"{value.W}:{value.H}";
    }

    internal string _hlsPlaylistType = "event";

    internal string _hlsSegmentFilename = "";

    internal string HlsSegmentFilename
    {
        get => _hlsSegmentFilename
            .Replace(":framesize:", $"{Scale.W}x{Scale.H}")
            .Replace(":type:", Type);
        set => _hlsSegmentFilename = value;
    }

    internal string _hlsPlaylistFilename = "";

    public bool IsHdr => VideoIsHdr();

    internal string HlsPlaylistFilename
    {
        get => _hlsPlaylistFilename
            .Replace(":framesize:", $"{Scale.W}x{Scale.H}")
            .Replace(":type:", Type);
        set => _hlsPlaylistFilename = value;
    }

    #endregion

    #region Setters

    public BaseVideo SetKiloBitrate(int kiloBitrate)
    {
        if (kiloBitrate < 0)
            throw new Exception("Wrong bitrate value");

        Bitrate = kiloBitrate;

        return this;
    }

    public bool VideoIsHdr()
    {
        return VideoStream?.PixelFormat.Contains("hdr") ?? false;
    }

    protected BaseVideo SetVideoCodec(string videoCodec)
    {
        CodecDto[] availableCodecs = AvailableCodecs;
        if (availableCodecs.All(codec => codec.Value != videoCodec))
            throw new Exception(
                $"Wrong video codec value for {videoCodec}, available formats are {string.Join(", ", AvailableCodecs.Select(codec => codec.Value))}");

        VideoCodec = availableCodecs.First(codec => codec.Value == videoCodec);

        return this;
    }

    public BaseVideo AddCustomArgument(string key, dynamic value)
    {
        _extraParameters[key] = $"\"{value}\"";
        return this;
    }

    internal BaseVideo AddFilter(string key, dynamic value)
    {
        _filters[key] = $"\"{value}\"";

        return this;
    }

    public BaseVideo AddOpts(string key, dynamic value)
    {
        _ops.Add(key, $"\"{value}\"");
        return this;
    }

    public BaseVideo SetMaxRate(int value)
    {
        MaxRate = value;
        return this;
    }

    public BaseVideo SetFrameRate(int value)
    {
        FrameRate = value;
        return this;
    }

    public BaseVideo SetBufferSize(int value)
    {
        BufferSize = value;
        return this;
    }

    public BaseVideo SetScale(string scale)
    {
        OutputWidth = scale.Split(":")[0].ToInt();
        ScaleValue = scale;
        return this;
    }

    public BaseVideo SetScale(int value)
    {
        OutputWidth = value;
        ScaleValue = $"{value}:-2";
        return this;
    }

    public BaseVideo SetScale(int width, int height)
    {
        OutputWidth = width;
        OutputHeight = height;
        ScaleValue = $"{width}:{height}";

        return this;
    }

    public BaseVideo SetConstantRateFactor(int value)
    {
        if (value < 0 || value > 51)
            throw new Exception("Wrong constant rate factor value");
        ConstantRateFactor = value;
        return this;
    }

    public BaseVideo SetFps(int value)
    {
        FrameRate = value;
        return this;
    }

    public BaseVideo SetColorSpace(string value)
    {
        PixelFormat = value;
        return this;
    }

    public BaseVideo SetPreset(string value)
    {
        if (!AvailablePresets.Contains(value))
            throw new Exception($"Wrong preset value for {value}, available formats are {string.Join(", ", AvailablePresets)}");
        Preset = value;
        return this;
    }

    public BaseVideo SetProfile(string value)
    {
        if (!AvailableProfiles.Contains(value))
            throw new Exception($"Wrong profile value for {value}, available formats are {string.Join(", ", AvailableProfiles)}");
        Profile = value;
        return this;
    }

    public BaseVideo SetTune(string value)
    {
        if (!AvailableTune.Contains(value))
            throw new Exception($"Wrong tune value for {value}, available formats are {string.Join(", ", AvailableTune)}");
        Tune = value;
        return this;
    }

    public BaseVideo AddOpts(string value)
    {
        // AddFilter(value, "");
        return this;
    }

    public BaseVideo SetHlsSegmentFilename(string value)
    {
        HlsSegmentFilename = value;
        return this;
    }

    public BaseVideo SetHlsPlaylistFilename(string value)
    {
        HlsPlaylistFilename = value;
        return this;
    }

    public BaseVideo FromStream(int value)
    {
        StreamIndex = value;
        return this;
    }

    public BaseVideo AllowHdr()
    {
        HdrAllowed = true;
        return this;
    }

    public BaseVideo ConvertHdrToSdr()
    {
        ConverToSdr = true;
        return this;
    }

    public override BaseVideo ApplyFlags()
    {
        if (MaxRate > 0)
            AddCustomArgument("-maxrate", MaxRate);
        if (BufferSize > 0)
            AddCustomArgument("-bufsize", BufferSize);

        if (Bitrate > 0)
        {
            AddCustomArgument("-b:v", Bitrate);
            AddCustomArgument("-keyint_min", KeyIntMin);
        }

        if (FrameRate > 0)
        {
            AddCustomArgument("-g", FrameRate);
            AddCustomArgument("-fps", FrameRate);
        }

        if (ConstantRateFactor > 0)
        {
            AddCustomArgument("-crf", ConstantRateFactor);
            AddCustomArgument("-cq:v", Convert.ToInt32(ConstantRateFactor * 1.12));
        }

        if (!string.IsNullOrEmpty(Preset))
            AddCustomArgument("-preset", Preset);
        if (!string.IsNullOrEmpty(Profile))
            AddCustomArgument("-profile:v", Profile);
        if (!string.IsNullOrEmpty(Tune))
            AddCustomArgument("-tune:v", Tune);

        return this;
    }

    public BaseVideo Build()
    {
        BaseVideo newStream = (BaseVideo)MemberwiseClone();

        newStream.IsVideo = true;

        newStream.VideoStream = VideoStreams.First();

        return newStream;
    }

    #endregion

    public abstract int GetPasses();

    public void AddToDictionary(Dictionary<string, dynamic> commandDictionary, int index)
    {
        commandDictionary["-map"] = $"[v{index}_hls_0]";
        commandDictionary["-c:v"] = VideoCodec.Value;

        commandDictionary["-map_metadata"] = -1;
        commandDictionary["-fflags"] = "+bitexact";
        commandDictionary["-flags:v"] = "+bitexact";
        commandDictionary["-flags:a"] = "+bitexact";
        commandDictionary["-flags:s"] = "+bitexact";

        commandDictionary["-movflags"] = "faststart";
        commandDictionary["-metadata"] = $"title=\"{Title}\"";

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
}