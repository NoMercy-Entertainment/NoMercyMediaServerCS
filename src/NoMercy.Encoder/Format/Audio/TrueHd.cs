using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class TrueHd : BaseAudio
{
    public TrueHd()
    {
        SetAudioCodec(AudioFormats.TrueHd);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.TrueHd
    ];

    protected override string[] AvailableContainers =>
    [
        VideoContainers.Mkv,
        VideoContainers.Mp4,
        VideoContainers.Hls
    ];
}