using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class DolbyDigitalPlus : BaseAudio
{
    public DolbyDigitalPlus()
    {
        SetAudioCodec(AudioFormats.DolbyDigitalPlus);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.Eac3
    ];

    protected override string[] AvailableContainers =>
    [
        VideoContainers.Mkv,
        VideoContainers.Mp4,
        VideoContainers.Hls
    ];
}