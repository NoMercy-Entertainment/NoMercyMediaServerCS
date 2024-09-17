using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Container;

public class WebM : BaseContainer
{
    public override ContainerDto ContainerDto => AvailableContainers.First(c => c.Name == VideoContainers.Webm);

    public WebM() : base()
    {
        SetContainer(VideoContainers.Webm);
        AddCustomArgument("-f", VideoFormats.Webm);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        VideoCodecs.Vp9, VideoCodecs.Vp9Nvenc
    ];
}