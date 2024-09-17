using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FFMpegCore;
using NoMercy.Encoder.Core;
using NoMercy.Encoder.Format.Rules;

namespace NoMercy.Encoder;

[Serializable]
public class MediaAnalysis(IMediaAnalysis mediaAnalysis, string path)
{
    public TimeSpan Duration { get; } = mediaAnalysis.Duration;
    public MediaFormat Format { get; } = mediaAnalysis.Format;
    public AudioStream? PrimaryAudioStream { get; } = mediaAnalysis.PrimaryAudioStream;
    public VideoStream? PrimaryVideoStream { get; } = mediaAnalysis.PrimaryVideoStream;
    public SubtitleStream? PrimarySubtitleStream { get; } = mediaAnalysis.PrimarySubtitleStream;
    public List<VideoStream> VideoStreams { get; } = mediaAnalysis.VideoStreams;
    public List<AudioStream> AudioStreams { get; } = mediaAnalysis.AudioStreams;
    public List<SubtitleStream> SubtitleStreams { get; } = mediaAnalysis.SubtitleStreams;
    public IReadOnlyList<string> ErrorData { get; } = mediaAnalysis.ErrorData;
    public string Path { get; } = path;
}

[Serializable]
public class FfMpeg : Classes
{
    internal string FfProbePath { get; set; } = NmSystem.AppFiles.FfProbePath;
    internal string FfmpegPath { get; set; } = NmSystem.AppFiles.FfmpegPath;

    internal MediaAnalysis? MediaAnalysis;

    public FfMpeg()
    {
    }

    public FfMpeg(string ffmpeg, string ffProbePath)
    {
        FfmpegPath = ffmpeg;
        FfProbePath = ffProbePath;
    }

    public void SetFfMpegDriver(string ffmpegDriver)
    {
        FfmpegPath = ffmpegDriver;
    }

    public void SetFFprobe(string ffprobe)
    {
        FfProbePath = ffprobe;
    }

    public string GetFfMpegDriver()
    {
        return FfmpegPath;
    }

    public string GetFFprobe()
    {
        return FfProbePath;
    }

    public string GetFfMpegVersion()
    {
        return FfmpegPath;
    }

    public string GetFFprobeVersion()
    {
        return FfProbePath;
    }

    public VideoAudioFile Open(string path)
    {
        // first ffprobe the file check for streams
        MediaAnalysis = new MediaAnalysis(FFProbe.Analyse(path), path);

        if (MediaAnalysis.VideoStreams.Count > 0)
            return new VideoFile(MediaAnalysis, FfmpegPath);

        if (MediaAnalysis.AudioStreams.Count > 0)
            return new AudioFile(MediaAnalysis, FfmpegPath);

        throw new Exception("No streams found");
    }

    public class FolderAndFile
    {
        public string HostFolder { get; set; }
        public string Filename { get; set; }
    }

    public VideoAudioFile Open(FolderAndFile? videoFile)
    {
        string inputFile = $"{videoFile?.HostFolder}{videoFile?.Filename}";
        return Open(inputFile);
    }

    public static async Task<string> Exec(string args, string? cwd = null, string? executable = null)
    {
        Process ffmpeg = new();
        ffmpeg.StartInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = executable ?? NmSystem.AppFiles.FfmpegPath,
            WorkingDirectory = cwd,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true
        };

        // Logger.Encoder(ffmpeg.StartInfo.WorkingDirectory  + " " + ffmpeg.StartInfo.FileName + " " + ffmpeg.StartInfo.Arguments);

        ffmpeg.Start();

        string error = await ffmpeg.StandardError.ReadToEndAsync();

        ffmpeg.Close();

        return error;
    }

    public static async Task<string> Run(string args, string cwd, ProgressMeta meta)
    {
        Process ffmpeg = new();
        ffmpeg.StartInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = NmSystem.AppFiles.FfmpegPath,
            WorkingDirectory = cwd,
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true
        };

        // Logger.Encoder(ffmpeg.StartInfo.WorkingDirectory  + " " + ffmpeg.StartInfo.FileName + " " + ffmpeg.StartInfo.Arguments);

        ffmpeg.Start();
        ffmpeg.BeginOutputReadLine();
        ffmpeg.BeginErrorReadLine();
        StringBuilder output = new();

        TimeSpan totalDuration = TimeSpan.Zero;
        bool durationFound = false;
        double progressPercentage = 0.0;
        TimeSpan currentTime = TimeSpan.Zero;

        StringBuilder output2 = new();
        ffmpeg.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
                if (!durationFound)
                {
                    // Extract duration from the log
                    Regex durationRegex = new(@"Duration:\s(\d{2}):(\d{2}):(\d{2})\.(\d+)");
                    Match durationMatch = durationRegex.Match(e.Data);

                    if (durationMatch.Success)
                    {
                        int hours = int.Parse(durationMatch.Groups[1].Value);
                        int minutes = int.Parse(durationMatch.Groups[2].Value);
                        int seconds = int.Parse(durationMatch.Groups[3].Value);
                        int milliseconds = int.Parse(durationMatch.Groups[4].Value);

                        totalDuration = new TimeSpan(0, hours, minutes, seconds, milliseconds * 10);
                        durationFound = true;
                        // Logger.Encoder($"Total Duration: {totalDuration}");
                    }
                }
        };

        ffmpeg.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                output.AppendLine(e.Data);

                output2.AppendLine(e.Data);

                if (e.Data.Contains("progress"))
                {
                    string[] x = output2.ToString().Split("\r\n");
                    IEnumerable<string[]> enumerable = x
                        .Select(y => y.Split("="))
                        .Where(y => y.Length == 2);

                    Dictionary<string, dynamic> enumerable2 = new();
                    foreach (string[] strings in enumerable)
                        enumerable2.Add(strings.FirstOrDefault() ?? "", strings.LastOrDefault() ?? "");

                    enumerable2.Add("totalDuration", totalDuration);

                    Regex progressRegex = new(@"(\d{2}):(\d{2}):(\d{2})\.(\d+)");
                    dynamic? progressMatch =
                        progressRegex.Match(enumerable2.TryGetValue("out_time", out dynamic? p) ? p : "");

                    if (progressMatch.Success)
                    {
                        int hours = int.Parse(progressMatch.Groups[1].Value);
                        int minutes = int.Parse(progressMatch.Groups[2].Value);
                        int seconds = int.Parse(progressMatch.Groups[3].Value);
                        int milliseconds = int.Parse(progressMatch.Groups[4].Value);

                        currentTime = new TimeSpan(0, hours, minutes, seconds, milliseconds / 100);
                        progressPercentage = currentTime.TotalMilliseconds / totalDuration.TotalMilliseconds * 100;
                    }

                    dynamic speed = enumerable2.TryGetValue("speed", out dynamic? s) ? s : 0;

                    string cleanedValue = speed.TrimEnd('x');
                    speed = double.Parse(cleanedValue, CultureInfo.InvariantCulture);

                    decimal fps = enumerable2.TryGetValue("fps", out dynamic? f) ? Convert.ToDecimal(f) : 0;
                    int frame = enumerable2.TryGetValue("frame", out dynamic? f2) ? int.Parse(f2) : 0;
                    string bitrate = enumerable2.TryGetValue("bitrate", out dynamic? b) ? b : "";

                    dynamic? remaining =
                        Math.Floor((totalDuration.TotalSeconds - currentTime.TotalSeconds) / (speed / 1000));

                    string? thumbFolder = Directory.GetDirectories(meta.BaseFolder, "*thumbs_*")
                        .FirstOrDefault();

                    string thumbnail = "";
                    string thumbnailFolder = "";
                    if (Directory.Exists(thumbFolder))
                    {
                        thumbnail = Directory.GetFiles(thumbFolder).LastOrDefault() ?? "";
                        thumbnail = Path.GetFileName(thumbnail);
                        thumbnailFolder = Path.GetFileNameWithoutExtension(thumbnail).Split("-").FirstOrDefault() ?? "";
                    }

                    Progress progress = new()
                    {
                        Percentage = progressPercentage,
                        Status = "Encoding",
                        CurrentTime = currentTime.TotalSeconds,
                        Duration = totalDuration.TotalSeconds,
                        Remaining = remaining,
                        RemainingHms = TimeSpan.FromSeconds(remaining == Single.PositiveInfinity ? 0 : remaining).ToString(),
                        Fps = Convert.ToDouble(fps / 1000),
                        Speed = Convert.ToDouble(speed / 1000),
                        Frame = frame,
                        Bitrate = bitrate,
                        HasGpu = false,
                        IsHDR = false,
                        VideoStreams = meta.VideoStreams,
                        AudioStreams = meta.AudioStreams,
                        SubtitleStreams = meta.SubtitleStreams,
                        Thumbnail = $"{meta.ShareBasePath}/{thumbnailFolder}/{thumbnail}",
                        Title = meta.Title,
                        Id = meta.Id
                    };

                    progress.RemainingSplit = progress.RemainingHms
                        .Split(":")
                        .Prepend("0")
                        .ToArray();

                    // Logger.Encoder(progress);

                    // Networking.SendToAll("Encoder", "dashboardHub", progress);
                    output2 = new StringBuilder();
                }
            }
        };

        await ffmpeg.WaitForExitAsync();

        ffmpeg.Close();

        // Networking.SendToAll("Encoder", "socket", new Progress
        // {
        //     Status = "done",
        //     Id = 0,
        // });

        // Logger.Encoder(output.ToString());

        return output.ToString();
    }
}