using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class Wav : BaseAudio
{
    public Wav()
    {
        SetAudioCodec(AudioFormats.Wav);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.PcmS16Le
    ];

    protected override string[] AvailableContainers =>
    [
        AudioContainers.Wav
    ];
}