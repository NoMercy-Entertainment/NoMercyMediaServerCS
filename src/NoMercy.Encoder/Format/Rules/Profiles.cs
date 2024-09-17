namespace NoMercy.Encoder.Format.Rules;

public static class AudioProfiles
{
}

public static class ImageProfiles
{
    // JPEG Profiles
    public static string Baseline = "baseline";
    public static string Extended = "extended";
    public static string Progressive = "progressive";
    public static string Lossless = "lossless";
    public static string Hierarchical = "hierarchical";

    // GIF Profiles
    public static string Gif87a = "gif87a";
    public static string Gif89a = "gif89a";

    // PNG Profiles
    public static string Png8 = "png8";
    public static string Png24 = "png24";
    public static string Png32 = "png32";
    public static string Png48 = "png48";
    public static string Png64 = "png64";

    // BMP Profiles
    public static string Bmp1 = "bmp1";
    public static string Bmp2 = "bmp2";
    public static string Bmp3 = "bmp3";

    // TIFF Profiles
}

public static class VideoProfiles
{
    // H.264 Profiles
    public static string Baseline = "baseline";
    public static string Main = "main";
    public static string High = "high";
    public static string High10 = "high10";
    public static string High444p = "high444p";

    // H.265 Profiles
    public static string High422 = "high422";
    public static string High444 = "high444";

    // VP9 Profiles
    public static string Unknown = "unknown";
    public static string Profile0 = "profile0";
    public static string Profile1 = "profile1";
    public static string Profile2 = "profile2";
    public static string Profile3 = "profile3";
}