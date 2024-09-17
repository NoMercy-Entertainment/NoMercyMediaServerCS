using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Container;

public class Srt : BaseContainer
{
    public Srt() : base()
    {
        SetContainer(SubtitleContainers.Srt);
        AddCustomArgument("-f", SubtitleFormats.Srt);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        SubtitleCodecs.Srt
    ];
}