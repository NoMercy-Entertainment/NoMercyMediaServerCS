namespace NoMercy.Encoder.Format.Rules;

public static class AudioContainers
{
    public static readonly string Mp3 = "mp3";
    public static readonly string Ogg = "ogg";
    public static readonly string Aac = "aac";
    public static readonly string Flac = "flac";
    public static readonly string M4a = "m4a";
    public static readonly string Wav = "wav";
}

public static class AudioFormats
{
    public static readonly string Mp3 = "libmp3lame";
    public static readonly string Ogg = "ogg";
    public static readonly string Aac = "aac";
    public static readonly string LibAac = "libfdk_aac";
    public static readonly string Flac = "flac";
    public static readonly string DolbyDigitalPlus = "eac3";
    public static readonly string DolbyDigital = "ac3";
    public static readonly string M4a = "m4a";
    public static readonly string TrueHd = "truehd";
    public static readonly string Wav = "pcm_s16le";
}

public static class ImageContainers
{
    public static readonly string Jpeg = "jpeg";
    public static readonly string Png = "png";
    public static readonly string Gif = "gif";
    public static readonly string Bmp = "bmp";
    public static readonly string Tiff = "tiff";
    public static readonly string Webp = "webp";
    public static readonly string Heif = "heif";
    public static readonly string Heic = "heic";
}

public static class SubtitleContainers
{
    public static readonly string Ass = "ass";
    public static readonly string Srt = "srt";
    public static readonly string WebVtt = "vtt";
}

public static class SubtitleFormats
{
    public static readonly string Ass = "ass";
    public static readonly string Srt = "srt";
    public static readonly string WebVtt = "vtt";
}

public static class VideoContainers
{
    public static readonly string Mkv = "mkv";
    public static readonly string Mp4 = "mp4";
    public static readonly string Hls = "m3u8";
    public static readonly string Webm = "webm";
    public static readonly string Flv = "flv";
}

public static class VideoFormats
{
    public static readonly string Mkv = "matroska";
    public static readonly string Mp4 = "mp4";
    public static readonly string Hls = "hls";
    public static readonly string Webm = "webm";
    public static readonly string Flv = "flv";
}