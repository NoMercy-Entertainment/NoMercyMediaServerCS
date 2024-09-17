namespace NoMercy.NmSystem;
public class MediaFile
{
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int Size { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Accessed { get; set; }
    public string Type { get; set; } = string.Empty;
    public MovieFileExtend? Parsed { get; init; }

    public FFprobeData? FFprobe { get; init; }
    // public Fingerprint? FingerPint { get; init; }
}