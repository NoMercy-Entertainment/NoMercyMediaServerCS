using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class Aac : BaseAudio
{
    public Aac(string audioCodec = "aac")
    {
        SetAudioCodec(audioCodec);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.Aac,
    ];

    protected override string[] AvailableContainers =>
    [
        AudioContainers.Aac,
        AudioContainers.M4a,

        VideoContainers.Mkv,
        VideoContainers.Mp4,
        VideoContainers.Flv,
        VideoContainers.Hls
    ];
}