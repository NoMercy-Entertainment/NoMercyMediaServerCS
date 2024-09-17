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

public static class AudioCodecs
{
    public static readonly Classes.CodecDto Aac = new()
    {
        Name = "Advanced Audio Coding",
        Value = "aac",
        SimpleValue = "aac",
        IsDefault = true
    };

    public static readonly Classes.CodecDto LibFdkAac = new()
    {
        Name = "Advanced Audio Coding",
        Value = "libfdk_aac",
        SimpleValue = "aac",
        IsDefault = false
    };

    public static readonly Classes.CodecDto Ac3 = new()
    {
        Name = "Dolby Digital",
        Value = "ac3",
        SimpleValue = "ac3",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Eac3 = new()
    {
        Name = "Dolby Digital Plus",
        Value = "eac3",
        SimpleValue = "eac3",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Flac = new()
    {
        Name = "Free Lossless Audio Codec",
        Value = "flac",
        SimpleValue = "flac",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Mp3 = new()
    {
        Name = "MP3",
        Value = "libmp3lame",
        SimpleValue = "mp3",
        IsDefault = true
    };

    public static readonly Classes.CodecDto LibOpus = new()
    {
        Name = "Opus Audio Codec",
        Value = "libopus",
        SimpleValue = "opus",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Opus = new()
    {
        Name = "Opus Audio Codec (Experimental)",
        Value = "opus",
        SimpleValue = "opus",
        RequiresStrict = true,
        IsDefault = false
    };

    public static readonly Classes.CodecDto TrueHd = new()
    {
        Name = "TrueHD",
        Value = "truehd",
        SimpleValue = "truehd",
        IsDefault = true
    };

    public static readonly Classes.CodecDto LibVorbis = new()
    {
        Name = "Vorbis Audio Codec",
        Value = "libvorbis",
        SimpleValue = "vorbis",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Vorbis = new()
    {
        Name = "Vorbis Audio Codec (experimental)",
        Value = "vorbis",
        SimpleValue = "vorbis",
        RequiresStrict = true,
        IsDefault = false
    };

    public static readonly Classes.CodecDto PcmS16Le = new()
    {
        Name = "PCM signed 16-bit little-endian",
        Value = "pcm_s16le",
        SimpleValue = "pcm",
        IsDefault = true
    };
}

public static class SubtitleCodecs
{
    public static readonly Classes.CodecDto Ass = new()
    {
        Name = "Ass",
        Value = "ass",
        SimpleValue = "ass",
        IsDefault = false
    };

    public static readonly Classes.CodecDto Srt = new()
    {
        Name = "SubRip",
        Value = "srt",
        SimpleValue = "srt",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Webvtt = new()
    {
        Name = "WebVTT",
        Value = "webvtt",
        SimpleValue = "vtt",
        IsDefault = false
    };
}

public static class ImageCodecs
{
    public static readonly Classes.CodecDto Jpeg = new()
    {
        Name = "Joint Photographic Experts Group",
        Value = "mjpeg",
        SimpleValue = "jpeg",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Png = new()
    {
        Name = "Portable Network Graphics",
        Value = "png",
        SimpleValue = "png",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Gif = new()
    {
        Name = "Graphics Interchange Format",
        Value = "gif",
        SimpleValue = "gif",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Bmp = new()
    {
        Name = "Bitmap",
        Value = "bmp",
        SimpleValue = "bmp",
        IsDefault = false
    };

    public static readonly Classes.CodecDto Tiff = new()
    {
        Name = "Tagged Image File Format",
        Value = "tiff",
        SimpleValue = "tiff",
        IsDefault = false
    };

    public static readonly Classes.CodecDto Webp = new()
    {
        Name = "WebP",
        Value = "webp",
        SimpleValue = "webp",
        IsDefault = true
    };

    public static readonly Classes.CodecDto Heif = new()
    {
        Name = "High Efficiency Image Format",
        Value = "heif",
        SimpleValue = "heif",
        IsDefault = false
    };

    public static readonly Classes.CodecDto Heic = new()
    {
        Name = "High Efficiency Image Container",
        Value = "heic",
        SimpleValue = "heic",
        IsDefault = false
    };
}