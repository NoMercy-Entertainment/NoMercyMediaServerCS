using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class Flac : BaseAudio
{
    public Flac()
    {
        SetAudioCodec(AudioFormats.Flac);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.Flac
    ];

    protected override string[] AvailableContainers =>
    [
        AudioContainers.Flac,

        VideoContainers.Mkv
    ];
}