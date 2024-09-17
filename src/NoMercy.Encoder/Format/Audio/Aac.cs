using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Audio;

public class Aac : BaseAudio
{
    public Aac(string audioCodec = "libfdk_aac")
    {
        SetAudioCodec(audioCodec);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        AudioCodecs.Aac,
        AudioCodecs.LibFdkAac
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