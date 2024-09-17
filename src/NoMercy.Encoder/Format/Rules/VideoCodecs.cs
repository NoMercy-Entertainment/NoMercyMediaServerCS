namespace NoMercy.Encoder.Format.Rules;

public static class VideoCodecs
{
    public static readonly Classes.CodecDto H264 = new()
    {
        Name = "H.264",
        Value = "libx264",
        SimpleValue = "h264",
        RequiresGpu = false,
        IsDefault = true
    };

    public static readonly Classes.CodecDto H264Nvenc = new()
    {
        Name = "H.264 (nvenc)",
        Value = "h264_nvenc",
        SimpleValue = "h264",
        RequiresGpu = true,
        IsDefault = false
    };

    public static readonly Classes.CodecDto H265 = new()
    {
        Name = "H.265",
        Value = "libx265",
        SimpleValue = "hevc",
        RequiresGpu = false,
        IsDefault = true
    };

    public static readonly Classes.CodecDto H265Nvenc = new()
    {
        Name = "H.265 (nvenc)",
        Value = "h265_nvenc",
        SimpleValue = "hevc",
        RequiresGpu = true,
        IsDefault = false
    };

    public static readonly Classes.CodecDto Vp9 = new()
    {
        Name = "vp9",
        Value = "vp9",
        SimpleValue = "vp9",
        RequiresGpu = false,
        IsDefault = true
    };

    public static readonly Classes.CodecDto Vp9Nvenc = new()
    {
        Name = "VP9 (nvenc)",
        Value = "libvpx-vp9",
        SimpleValue = "vp9",
        RequiresGpu = true,
        IsDefault = false
    };
}