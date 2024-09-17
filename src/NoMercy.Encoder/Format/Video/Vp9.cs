using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Video;

public class Vp9 : BaseVideo
{
    protected internal override bool BFramesSupport => true;
    internal int Passes { get; set; } = 2;

    public Vp9(string videoCodec = "vp9")
    {
        throw new NotImplementedException("Vp9 is not implemented yet.");
        SetVideoCodec(videoCodec);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        VideoCodecs.Vp9,
        VideoCodecs.Vp9Nvenc
    ];

    protected internal override string[] AvailableContainers =>
    [
        VideoContainers.Mkv, VideoContainers.Webm,
        VideoContainers.Flv, VideoContainers.Hls
    ];

    protected internal override string[] AvailablePresets =>
    [
        VideoPresets.VeryFast, VideoPresets.Faster, VideoPresets.Fast,
        VideoPresets.Medium,
        VideoPresets.Slow, VideoPresets.Slower, VideoPresets.VerySlow
    ];

    protected internal override string[] AvailableProfiles =>
    [
        VideoProfiles.Unknown, VideoProfiles.Profile0, VideoProfiles.Profile1,
        VideoProfiles.Profile2, VideoProfiles.Profile3
    ];

    protected internal override string[] AvailableTune =>
    [
        VideoTunes.Hq, VideoTunes.Li,
        VideoTunes.Ull, VideoTunes.Lossless
    ];

    protected internal override string[] AvailableLevels => [];

    public override int GetPasses()
    {
        return 0 == Bitrate ? 1 : Passes;
    }
}