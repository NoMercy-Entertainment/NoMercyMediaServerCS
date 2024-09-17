namespace NoMercy.Encoder.Core;

public class AudioFile(MediaAnalysis? fMediaAnalysis, string ffmpegPath) : VideoAudioFile(fMediaAnalysis, ffmpegPath) {
    internal override bool IsAudio => true;

}