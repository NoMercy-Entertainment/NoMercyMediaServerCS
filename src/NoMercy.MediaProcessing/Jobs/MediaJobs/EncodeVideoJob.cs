using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Encoder;
using NoMercy.MediaProcessing.Libraries;
using NoMercy.Encoder.Core;
using NoMercy.Encoder.Format.Audio;
using NoMercy.Encoder.Format.Container;
using NoMercy.Encoder.Format.Image;
using NoMercy.Encoder.Format.Rules;
using NoMercy.Encoder.Format.Subtitle;
using NoMercy.Encoder.Format.Video;
using NoMercy.NmSystem;
using Vtt = NoMercy.Encoder.Format.Subtitle.Vtt;

namespace NoMercy.MediaProcessing.Jobs.MediaJobs;

public class EncodeVideoJob : AbstractEncoderJob
{
    public override string QueueName => "encoder";
    public override int Priority => 10;

    public override async Task Handle()
    {
        await using MediaContext context = new();
        JobDispatcher jobDispatcher = new();

        LibraryRepository libraryRepository = new(context);
        LibraryManager libraryManager = new(libraryRepository, jobDispatcher);
        
        Folder? folder = await libraryRepository.GetLibraryFolder(FolderId);
        if (folder is null) return;
        
        List<EncoderProfile> profiles = folder.EncoderProfileFolder
            .Select(e => e.EncoderProfile)
            .ToList();
        
        if (!profiles.Any()) return;
        
        var movie = folder.FolderLibraries.Any(x => x.Library.Type == "movie")
            ? await context.Movies
                .FirstOrDefaultAsync(x => x.Id == Id) 
            : null;
        
        var episode = folder.FolderLibraries.Any(x => x.Library.Type == "tv" || x.Library.Type == "anime")
            ? await context.Episodes
                .Include(x => x.Tv)
                .FirstOrDefaultAsync(x => x.Id == Id)
            : null; 
        
        if (movie is null && episode is null) return;
        
        var folderName = (movie?.CreateFolderName().Replace("/", "") 
                          ?? episode!.Tv.CreateFolderName().Replace("/", "") + episode.CreateFolderName())
            .Replace("/", Path.DirectorySeparatorChar.ToString());
        
        var title = movie?.CreateTitle() ?? episode!.CreateTitle();
        var fileName = movie?.CreateFileName() ?? episode!.CreateFileName();
        var basePath = Path.Combine(folder.Path, folderName);

        foreach (var profile in profiles)
        {
            BaseContainer container = profile.Container switch
            {
                "mkv" => new Mkv(),
                "Mp4" => new Mp4(),
                "Hls" => new Hls().SetHlsFlags("independent_segments"),
                _ => new Hls().SetHlsFlags("independent_segments")
            };
            
            BuildVideoStreams(profile, container);
            BuildAudioStreams(profile, container);
            BuildSubtitleStreams(profile, container, fileName);
                
            BaseImage sprite = new Sprite()
                .SetScale(320)
                .SetFilename("thumbs_:framesize:");
            container.AddStream(sprite);
            
            var ffmpeg = new FfMpeg()
                .Open(InputFile);
        
            ffmpeg.SetBasePath(basePath);
            ffmpeg.SetTitle(title);
            ffmpeg.ToFile(fileName);
        
            ffmpeg.AddContainer(container);
        
            ffmpeg.Build();
        
            var fullCommand = ffmpeg.GetFullCommand();
            Logger.Encoder(fullCommand);
        
            var progressMeta = new ProgressMeta
            {
                Id = movie?.Id ?? episode!.Id,
                Title = title,
                BaseFolder = basePath,
                ShareBasePath = folder.Id + "/" + folderName,
                AudioStreams = container.AudioStreams.Select(x => $"{x.StreamIndex}:{x.Language}_{x.AudioCodec.SimpleValue}").ToList(),
                VideoStreams = container.VideoStreams.Select(x => $"{x.StreamIndex}:{x.Scale.W}x{x.Scale.H}_{x.VideoCodec.SimpleValue}").ToList(),
                SubtitleStreams = container.SubtitleStreams.Select(x => $"{x.StreamIndex}:{x.Language}_{x.SubtitleCodec.SimpleValue}").ToList(),
                HasGpu = container.VideoStreams.Any(x =>
                    x.VideoCodec.Value == VideoCodecs.H264Nvenc.Value || x.VideoCodec.Value == VideoCodecs.H265Nvenc.Value),
                IsHDR = container.VideoStreams.Any(x => x.IsHdr),
            };
        
            // var result = await ffmpeg.Run(fullCommand, basePath, progressMeta);
            // Logger.Encoder(result);
        
            await sprite.BuildSprite(progressMeta);
        
            container.BuildMasterPlaylist();
        }
    }

    private void BuildVideoStreams(EncoderProfile profile, BaseContainer container)
    {
        // var stream = new X264(VideoCodecs.H264Nvenc.Value)
        //     .SetScale(FrameSizes._4k.Width, -2)
        //     .SetConstantRateFactor(20)
        //     .ConvertHdrToSdr()
        //     .SetHlsSegmentFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
        //     .SetHlsPlaylistFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
        //     .SetColorSpace(ColorSpaces.Yuv420p)
        //     .SetPreset(VideoPresets.Fast)
        //     .SetTune(VideoTunes.Hq)
        //     .AddOpts("no-scenecut")
        //     .AddOpts("keyint=48")
        //     .AddCustomArgument("-x264opts", "no-scenecut");
        //
        // container.AddStream(stream);
        
        var stream2 = new X264(VideoCodecs.H264Nvenc.Value)
            .SetScale(FrameSizes._1080p.Width)
            .SetConstantRateFactor(20)
            .ConvertHdrToSdr()
            .SetHlsSegmentFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
            .SetHlsPlaylistFilename(":type:_:framesize:_SDR/:type:_:framesize:_SDR")
            .SetColorSpace(ColorSpaces.Yuv420p)
            .SetPreset(VideoPresets.Fast)
            .SetTune(VideoTunes.Hq)
            .AddOpts("no-scenecut")
            .AddOpts("keyint", 48)
            .AddCustomArgument("-x264opts", "no-scenecut");
        
        container.AddStream(stream2);
    }

    private void BuildAudioStreams(EncoderProfile profile, BaseContainer container)
    {
        BaseAudio stream = new Aac()
            .SetAudioChannels(2)
            .SetAllowedLanguages([
                Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita, Languages.Spa,
                Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
            ])
            .SetHlsSegmentFilename(":type:_:language:_:codec:/:type:_:language:_:codec:")
            .SetHlsPlaylistFilename(":type:_:language:_:codec:/:type:_:language:_:codec:");
        
        container.AddStream(stream);
    }

    private void BuildSubtitleStreams(EncoderProfile profile, BaseContainer container, string fileName)
    {
            BaseSubtitle stream = new Vtt()
                .SetAllowedLanguages([
                    Languages.Dut, Languages.Eng, Languages.Jpn, Languages.Fre, Languages.Ger, Languages.Ita, Languages.Spa,
                    Languages.Por, Languages.Rus, Languages.Kor, Languages.Chi, Languages.Ara
                ])
                .SetHlsSegmentFilename(":type:_:language:_:codec:/_:language:_:codec:")
                .SetHlsPlaylistFilename($"subtitles/{fileName}.:language:.:variant:");
            
            container.AddStream(stream);
    }
}