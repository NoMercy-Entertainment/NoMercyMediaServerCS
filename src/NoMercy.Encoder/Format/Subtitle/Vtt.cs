using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Subtitle;

public class Vtt : BaseSubtitle
{
    public Vtt(string subtitleCodec = "webvtt")
    {
        SetSubtitleCodec(subtitleCodec);
        AddCustomArgument("-f", subtitleCodec);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        SubtitleCodecs.Webvtt
    ];

    protected override string[] AvailableContainers =>
    [
        SubtitleContainers.WebVtt
    ];
}