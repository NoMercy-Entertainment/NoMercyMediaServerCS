using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.Networking;
using NoMercy.NmSystem;

namespace NoMercy.Api.Controllers.Socket;

public class Song
{
    [JsonProperty("color_palette")] public ColorPalette ColorPalette { get; set; } = new();
    [JsonProperty("cover")] public string Cover { get; set; } = string.Empty;
    [JsonProperty("date")] public DateTime Date { get; set; }
    [JsonProperty("disc")] public int Disc { get; set; }
    [JsonProperty("duration")] public string Duration { get; set; } = string.Empty;
    [JsonProperty("favorite")] public bool Favorite { get; set; }
    [JsonProperty("filename")] public string Filename { get; set; } = string.Empty;
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("folder_id")] public Ulid FolderId { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("origin")] public string Origin { get; set; } = "local";
    [JsonProperty("path")] public string Path { get; set; } = string.Empty;
    [JsonProperty("quality")] public int Quality { get; set; }
    [JsonProperty("track")] public int Track { get; set; }

    [JsonProperty("type")] public string Type { get; set; } = "audio";

    [JsonProperty("lyrics")] public Lyric[]? Lyrics { get; set; }
    [JsonProperty("artist_track")] public Track[] ArtistTrack { get; set; } = [];
    [JsonProperty("album_track")] public Track[] AlbumTrack { get; set; } = [];
    [JsonProperty("full_cover")] public string FullCover { get; set; } = string.Empty;
}

public class Lyric
{
    [JsonProperty("text")] public string Text { get; set; } = string.Empty;
    [JsonProperty("time")] public LineTime Time { get; set; } = new();

    public class LineTime
    {
        [JsonProperty("total")] public double Total { get; set; }
        [JsonProperty("minutes")] public int Minutes { get; set; }
        [JsonProperty("seconds")] public int Seconds { get; set; }
        [JsonProperty("hundredths")] public int Hundredths { get; set; }
    }
}

public class Track
{
    [JsonProperty("color_palette")] public ColorPalette ColorPalette { get; set; } = new();
    [JsonProperty("cover")] public string Cover { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("folder")] public string Folder { get; set; } = string.Empty;
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("library_id")] public Ulid LibraryId { get; set; }
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("origin")] public string Origin { get; set; } = "local";
}

public class ColorPalette
{
    [JsonProperty("cover")] public PaletteColors Cover { get; set; } = new();
}

public class PlayerState
{
    [JsonProperty("play_State")] public string PlayState { get; set; } = "idle";
    [JsonProperty("time")] public int Time { get; set; }
    [JsonProperty("volume")] public int Volume { get; set; } = 20;
    [JsonProperty("muted")] public bool Muted { get; set; }
    [JsonProperty("shuffle")] public bool Shuffle { get; set; }
    [JsonProperty("repeat")] public string Repeat { get; set; } = "none";
    [JsonProperty("percentage")] public int Percentage { get; set; }
    [JsonProperty("current_device")] public string? CurrentDevice { get; set; }
    [JsonProperty("current_playlist")] public string? CurrentPlaylist { get; set; }
    [JsonProperty("currentItem")] public Song? CurrentItem { get; set; }
    [JsonProperty("queue")] public IEnumerable<Song> Queue { get; set; } = [];
    [JsonProperty("backlog")] public IEnumerable<Song> Backlog { get; set; } = [];
}

public class VideoHub : ConnectionHub
{
    private static readonly ConcurrentDictionary<Guid, string> CurrentDevices = new();
    private static readonly ConcurrentDictionary<Guid, PlayerState> PlayerState = new();

    public VideoHub(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
    }

    public async Task SetTime(VideoProgressRequest request)
    {
        Guid userId = Guid.Parse(Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        User? user = ClaimsPrincipleExtensions.Users.FirstOrDefault(x => x.Id == userId);

        if (user is null) return;

        UserData userdata = new()
        {
            UserId = user.Id,
            Type = request.PlaylistType,
            Time = request.Time,
            Audio = request.Audio,
            Subtitle = request.Subtitle,
            SubtitleType = request.SubtitleType,
            VideoFileId = Ulid.Parse(request.VideoId),
            MovieId = request.PlaylistType == "movie" ? request.TmdbId : null,
            TvId = request.PlaylistType == "tv" ? request.TmdbId : null,
            CollectionId = request.PlaylistType == "collection" ? request.TmdbId : null,
            SpecialId = request.SpecialId
        };

        await using MediaContext mediaContext = new();
        await mediaContext.UserData.Upsert(userdata)
            .On(x => new { x.UserId, x.VideoFileId })
            .WhenMatched((uds, udi) => new UserData
            {
                Id = uds.Id,
                Time = udi.Time,
                Audio = udi.Audio,
                Subtitle = udi.Subtitle,
                SubtitleType = udi.SubtitleType,
                LastPlayedDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")
            })
            .RunAsync();
    }

    public async Task RemoveWatched(VideoProgressRequest request)
    {
        Guid userId = Guid.Parse(Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

        User? user = ClaimsPrincipleExtensions.Users.FirstOrDefault(x => x.Id == userId);

        if (user is null) return;

        await using MediaContext mediaContext = new();
        UserData[] userdata = await mediaContext.UserData
            .Where(x => x.UserId == user.Id)
            .Where(x => x.Type == request.PlaylistType)
            .Where(x => x.MovieId == request.TmdbId
                        || x.TvId == request.TmdbId
                        || x.SpecialId == request.SpecialId
                        || x.CollectionId == request.TmdbId)
            .ToArrayAsync();

        mediaContext.UserData.RemoveRange(userdata);

        await mediaContext.SaveChangesAsync();
    }

    public Task CreateSession(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task Disconnect(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task Connect(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task Party(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task JoinSession(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task LeaveSession(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task PartyTime(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task PartyPlay(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task PartyPause(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public Task PartyItem(VideoProgressRequest request)
    {
        return Task.CompletedTask;
    }

    public void Log(dynamic? request)
    {
        Logger.Socket(request);
    }

    public class VideoProgressRequest
    {
        [JsonProperty("app")] public int AppId { get; set; }
        [JsonProperty("video_id")] public string VideoId { get; set; } = string.Empty;
        [JsonProperty("tmdb_id")] public int TmdbId { get; set; }
        [JsonProperty("playlist_type")] public string PlaylistType { get; set; } = string.Empty;
        [JsonProperty("video_type")] public string VideoType { get; set; } = string.Empty;
        [JsonProperty("time")] public int Time { get; set; }
        [JsonProperty("audio")] public string Audio { get; set; } = string.Empty;
        [JsonProperty("subtitle")] public string Subtitle { get; set; } = string.Empty;
        [JsonProperty("subtitle_type")] public string SubtitleType { get; set; } = string.Empty;
        [JsonProperty("special_id")] public Ulid? SpecialId { get; set; }
    }

    private PlayerState MusicPlayerState()
    {
        User? user = Context.User.User();

        if (user is null)
        {
            Logger.Socket("Creating new player state");
            return new PlayerState();
        }

        PlayerState? playerState = PlayerState.FirstOrDefault(p => p.Key == user.Id).Value;

        if (playerState != null)
        {
            if (playerState.CurrentItem is not null) playerState.CurrentItem.Lyrics = null;
            return playerState;
        }

        Logger.Socket("Creating new player state");
        playerState = new PlayerState();

        PlayerState.TryAdd(user.Id, playerState);

        return playerState;
    }

    public List<Device> ConnectedDevices()
    {
        User? user = Context.User.User();

        return Networking.Networking.SocketClients.Values
            .Where(x => x.Sub.Equals(user?.Id))
            .Select(c => new Device
            {
                Name = c.Name,
                Ip = c.Ip,
                DeviceId = c.DeviceId,
                Browser = c.Browser,
                Os = c.Os,
                Model = c.Model,
                Type = c.Type,
                Version = c.Version,
                Id = c.Id,
                CustomName = c.CustomName
            })
            .ToList();
    }

    public class CurrentDeviceRequest
    {
        [JsonProperty("deviceId")] public string DeviceId { get; set; } = string.Empty;
    }

    public void SetCurrentDevice(CurrentDeviceRequest request)
    {
        User? user = Context.User.User();

        PlayerState state = MusicPlayerState();
        state.CurrentDevice = request.DeviceId;

        CurrentDevices.TryAdd(user!.Id, request.DeviceId);

        Clients.All.SendAsync("CurrentDevice", request.DeviceId);

        if (state.Muted) Clients.All.SendAsync("Mute", state.Muted);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

        List<Device> connectedDevices = ConnectedDevices();
        PlayerState state = MusicPlayerState();

        // if (state.CurrentItem is null)
        // {
        //     state.Time = 0;
        //     state.Volume = 20;
        // }

        if (connectedDevices.Any(c => c.DeviceId == state.CurrentDevice) == false) await Clients.All.SendAsync("Pause");
    }

    public void CurrentDevice()
    {
        PlayerState state = MusicPlayerState();

        Clients.Caller.SendAsync("CurrentDevice", state.CurrentDevice);

        if (state.Muted) Clients.All.SendAsync("Mute", state.Muted);

        switch (state.PlayState)
        {
            case "play":
                Clients.Others.SendAsync("Play");
                break;
            case "pause":
                Clients.Others.SendAsync("Pause");
                break;
            case "stop":
                Clients.Others.SendAsync("Stop");
                break;
        }

        Clients.All.SendAsync("Volume", state.Volume);
    }

    public async Task Play()
    {
        PlayerState state = MusicPlayerState();
        state.PlayState = "play";

        await Clients.Others.SendAsync("Play");
    }

    public async Task Stop()
    {
        PlayerState state = MusicPlayerState();
        state.PlayState = "stop";
        state.CurrentItem = null;
        state.Queue = [];
        state.Backlog = [];

        Logger.Socket(state);

        await Clients.All.SendAsync("Stop");
        await Clients.All.SendAsync("State", state);
        await Clients.All.SendAsync("Queue", state.Queue);
        await Clients.All.SendAsync("Backlog", state.Backlog);
    }

    public async Task Pause()
    {
        PlayerState state = MusicPlayerState();
        state.PlayState = "pause";

        // Logger.Socket(state.State);

        await Clients.Others.SendAsync("Pause");
    }

    public async Task Next()
    {
        await Clients.Others.SendAsync("Next");
    }

    public async Task Previous()
    {
        await Clients.Others.SendAsync("Previous");
    }

    public async Task Shuffle(bool shuffle)
    {
        PlayerState state = MusicPlayerState();
        state.Shuffle = shuffle;

        await Clients.Others.SendAsync("Shuffle");
    }

    public async Task Repeat(string repeat)
    {
        PlayerState state = MusicPlayerState();
        state.Repeat = repeat;

        // Logger.Socket(state.Repeat.ToString());

        await Clients.Others.SendAsync("Repeat");
    }

    public async Task Mute(bool mute)
    {
        PlayerState state = MusicPlayerState();
        state.Muted = mute;

        // Logger.Socket(state.Muted.ToString());

        await Clients.Others.SendAsync("Mute", mute);
    }

    public async Task Volume(int volume)
    {
        PlayerState state = MusicPlayerState();
        state.Volume = volume;

        await Clients.Others.SendAsync("Volume", volume);
    }

    public async Task SeekTo(int value)
    {
        PlayerState state = MusicPlayerState();
        state.Time = value;

        // Logger.Socket(state.Percentage.ToString());

        await Clients.Others.SendAsync("SeekTo", value);
    }

    public async Task CurrentTime(int time)
    {
        PlayerState state = MusicPlayerState();
        state.Time = time;

        // Logger.Socket(state.Time.ToString(CultureInfo.InvariantCulture));

        await Clients.Others.SendAsync("CurrentTime", time);
    }

    public async Task CurrentItem(Song? currentItem)
    {
        PlayerState state = MusicPlayerState();
        state.CurrentItem = currentItem;

        await Clients.Others.SendAsync("CurrentItem", currentItem);
        await Clients.All.SendAsync("CurrentDevice", state.CurrentDevice);
    }

    public async Task Queue(Dictionary<string, Song> queue)
    {
        PlayerState state = MusicPlayerState();
        state.Queue = queue.Values;

        await Clients.Others.SendAsync("Queue", queue.Values);
    }

    public async Task Backlog(Dictionary<string, Song> backlog)
    {
        PlayerState state = MusicPlayerState();
        state.Backlog = backlog.Values;

        await Clients.Others.SendAsync("Backlog", backlog.Values);
    }

    public PlayerState State()
    {
        PlayerState state = MusicPlayerState();
        return state;
    }
}