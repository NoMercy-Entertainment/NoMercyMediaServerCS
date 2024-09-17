using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Subtitle;

public class Srt : BaseSubtitle
{
    public Srt(string subtitleCodec = "srt")
    {
        SetSubtitleCodec(subtitleCodec);
        AddCustomArgument("-f", subtitleCodec);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        SubtitleCodecs.Srt
    ];

    protected override string[] AvailableContainers =>
    [
        SubtitleContainers.Srt
    ];
}