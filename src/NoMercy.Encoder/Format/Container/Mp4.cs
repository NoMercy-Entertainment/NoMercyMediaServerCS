using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Container;

public class Mp4 : BaseContainer
{
    public override ContainerDto ContainerDto => AvailableContainers.First(c => c.Name == VideoContainers.Mp4);

    public Mp4() : base()
    {
        SetContainer(VideoContainers.Mp4);
        AddCustomArgument("-f", VideoFormats.Mp4);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        VideoCodecs.H264, VideoCodecs.H264Nvenc,
        VideoCodecs.H265, VideoCodecs.H265Nvenc,
        VideoCodecs.Vp9, VideoCodecs.Vp9Nvenc
    ];

    public override Mp4 ApplyFlags()
    {
        base.ApplyFlags();
        AddCustomArgument("-bsf:v", "h264_mp4toannexb");
        AddCustomArgument("-use_wallclock_as_timestamps", 1);
        return this;
    }
}