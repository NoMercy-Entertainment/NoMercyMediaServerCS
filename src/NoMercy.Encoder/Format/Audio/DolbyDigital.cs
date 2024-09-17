using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class DolbyDigital : BaseAudio
{
    public DolbyDigital()
    {
        SetAudioCodec(AudioFormats.DolbyDigital);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.Ac3
    ];

    protected override string[] AvailableContainers =>
    [
        VideoContainers.Mkv,
        VideoContainers.Mp4,
        VideoContainers.Hls
    ];
}