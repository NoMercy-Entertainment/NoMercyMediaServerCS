using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Container;

public class Mkv : BaseContainer
{
    public override ContainerDto ContainerDto => AvailableContainers.First(c => c.Name == VideoContainers.Mkv);

    public Mkv() : base()
    {
        SetContainer(VideoContainers.Mkv);
        AddCustomArgument("-f", VideoFormats.Mkv);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        VideoCodecs.H264, VideoCodecs.H264Nvenc,
        VideoCodecs.H265, VideoCodecs.H265Nvenc,
        VideoCodecs.Vp9, VideoCodecs.Vp9Nvenc
    ];
}