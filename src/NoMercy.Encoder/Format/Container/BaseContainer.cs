using NoMercy.Encoder.Core;
using NoMercy.Encoder.Format.Audio;
using NoMercy.Encoder.Format.Image;
using NoMercy.Encoder.Format.Rules;
using NoMercy.Encoder.Format.Subtitle;
using NoMercy.Encoder.Format.Video;

namespace NoMercy.Encoder.Format.Container;

public class BaseContainer : Classes
{
    #region Properties

    public virtual ContainerDto ContainerDto { get; set; } = AvailableContainers.First(c => c.IsDefault);

    public MediaAnalysis? MediaAnalysis;
    public readonly List<BaseVideo> VideoStreams = [];
    public readonly List<BaseAudio> AudioStreams = [];
    public readonly List<BaseSubtitle> SubtitleStreams = [];
    public readonly List<BaseImage> ImageStreams = [];

    public static ContainerDto[] AvailableContainers =>
    [
        new ContainerDto { Name = AudioContainers.Aac, Type = "audio", IsDefault = false },
        new ContainerDto { Name = AudioContainers.Flac, Type = "audio", IsDefault = false },
        new ContainerDto { Name = AudioContainers.Mp3, Type = "audio", IsDefault = true },
        new ContainerDto { Name = VideoContainers.Hls, Type = "video", IsDefault = false },
        new ContainerDto { Name = VideoContainers.Mkv, Type = "video", IsDefault = false },
        new ContainerDto { Name = VideoContainers.Mp4, Type = "video", IsDefault = true },
        new ContainerDto { Name = VideoContainers.Webm, Type = "video", IsDefault = false }
    ];

    internal readonly Dictionary<string, dynamic> _extraParameters = [];
    private readonly Dictionary<string, dynamic> _filters = [];
    private readonly Dictionary<string, dynamic> _ops = [];
    protected internal readonly Dictionary<int, dynamic> Streams = [];

    protected virtual CodecDto[] AvailableCodecs => [];
    protected virtual string[] AvailablePresets => [];
    protected virtual string[] AvailableProfiles => [];
    protected virtual string[] AvailableTunes => [];
    protected virtual string[] AvailableLevels => [];

    #endregion

    #region Setters

    protected BaseContainer SetContainer(string videoContainer)
    {
        ContainerDto[] availableCodecs = AvailableContainers;
        if (availableCodecs.All(container => container.Name != videoContainer))
            throw new Exception(
                $"Wrong video container value for {videoContainer}, available formats are {string.Join(", ", AvailableContainers.Select(container => container.Name))}");

        ContainerDto = availableCodecs.First(container => container.Name == videoContainer);

        return this;
    }

    public BaseContainer AddCustomArgument(string key, dynamic value)
    {
        _extraParameters[key] = value;
        return this;
    }

    public BaseContainer AddCustomArgument(string value)
    {
        _extraParameters.Add(value, "");
        return this;
    }

    public BaseContainer AddOpts(string key, dynamic value)
    {
        _ops[key] = value;
        return this;
    }

    public BaseContainer AddStream(BaseVideo stream)
    {
        stream.IsVideo = true;
        Streams.Add(Streams.Count, stream);
        return this;
    }

    public BaseContainer AddStream(BaseAudio stream)
    {
        stream.IsAudio = true;
        Streams.Add(Streams.Count, stream);
        return this;
    }

    public BaseContainer AddStream(BaseSubtitle stream)
    {
        stream.IsSubtitle = true;
        Streams.Add(Streams.Count, stream);
        return this;
    }

    public BaseContainer AddStream(BaseImage stream)
    {
        stream.IsImage = true;
        Streams.Add(Streams.Count, stream);
        return this;
    }

    public override BaseContainer ApplyFlags()
    {
        return this;
    }

    #endregion

    public void BuildMasterPlaylist()
    {
        HlsPlaylistGenerator.Build(BasePath, FileName);
    }
}