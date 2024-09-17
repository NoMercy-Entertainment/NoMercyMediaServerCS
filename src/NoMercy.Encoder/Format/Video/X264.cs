using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder.Format.Video;

// https://trac.ffmpeg.org/wiki/Encode/H.264

public class X264 : BaseVideo
{
    protected internal override bool BFramesSupport => true;
    protected internal override int Modulus => 2;
    internal int Passes { get; set; } = 2;

    public X264(string videoCodec = "libx264")
    {
        SetVideoCodec(videoCodec);
    }

    protected override CodecDto[] AvailableCodecs =>
    [
        VideoCodecs.H264,
        VideoCodecs.H264Nvenc
    ];

    protected internal override string[] AvailableContainers =>
    [
        VideoContainers.Mkv,
        VideoContainers.Mp4,
        VideoContainers.Hls
    ];

    protected internal override string[] AvailablePresets
    {
        get
        {
            if ("h264_nvenc" == VideoCodec.Value)
                return
                [
                    VideoPresets.Default, VideoPresets.Slow, VideoPresets.Medium, VideoPresets.Fast,
                    VideoPresets.Hp, VideoPresets.Hq, VideoPresets.Ll, VideoPresets.Llhq, VideoPresets.Llhp,
                    VideoPresets.Lossless,
                    VideoPresets.P1, VideoPresets.P2, VideoPresets.P3, VideoPresets.P4, VideoPresets.P5,
                    VideoPresets.P6, VideoPresets.P7
                ];

            return
            [
                VideoPresets.UltraFast, VideoPresets.SuperFast, VideoPresets.VeryFast,
                VideoPresets.Faster, VideoPresets.Fast, VideoPresets.Medium,
                VideoPresets.Slow, VideoPresets.Slower, VideoPresets.VerySlow,
                VideoPresets.Placebo
            ];
        }
    }

    protected internal override string[] AvailableProfiles
    {
        get
        {
            if ("h264_nvenc" == VideoCodec.Value)
                return
                [
                    VideoProfiles.Baseline, VideoProfiles.Main, VideoProfiles.High,
                    VideoProfiles.High10, VideoProfiles.High422, VideoProfiles.High444p
                ];

            return
            [
                VideoProfiles.Baseline, VideoProfiles.Main, VideoProfiles.High,
                VideoProfiles.High10, VideoProfiles.High444p
            ];
        }
    }

    protected internal override string[] AvailableTune
    {
        get
        {
            if ("h264_nvenc" == VideoCodec.Value)
                return
                [
                    VideoTunes.Hq, VideoTunes.Li,
                    VideoTunes.Ull, VideoTunes.Lossless
                ];

            return
            [
                VideoTunes.Film, VideoTunes.Animation,
                VideoTunes.Grain, VideoTunes.StillImage,
                VideoTunes.Fastdecode, VideoTunes.Zerolatency,
                VideoTunes.Psnr, VideoTunes.Ssim
            ];
        }
    }

    protected internal override string[] AvailableLevels =>
    [
        "auto",
        "1", "1.0", "1b", "1.0b", "1.1", "1.2", "1.3",
        "2", "2.0", "2.1", "2.2",
        "3", "3.0", "3.1", "3.2",
        "4", "4.0", "4.1", "4.2",
        "5", "5.0", "5.1", "5.2",
        "6.0", "6.1", "6.2"
    ];

    public X264 SetPasses(int passes)
    {
        Passes = passes;
        return this;
    }

    public override int GetPasses()
    {
        return 0 == Bitrate ? 1 : Passes;
    }
}