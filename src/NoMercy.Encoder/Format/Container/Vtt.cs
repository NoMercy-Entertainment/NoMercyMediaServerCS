using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Container;

public class Vtt : BaseContainer
{
    public Vtt() : base()
    {
        SetContainer(SubtitleContainers.WebVtt);
        AddCustomArgument("-f", SubtitleFormats.WebVtt);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        SubtitleCodecs.Webvtt
    ];
}