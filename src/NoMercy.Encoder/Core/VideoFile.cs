namespace NoMercy.Encoder.Core;

public class VideoFile(MediaAnalysis? fMediaAnalysis, string ffmpegPath) : VideoAudioFile(fMediaAnalysis, ffmpegPath) {
    internal override bool IsVideo => true;

}