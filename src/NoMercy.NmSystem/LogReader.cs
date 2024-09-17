using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Serilog.Events;

namespace NoMercy.NmSystem;

public class LogEntry
{
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;

    // [JsonProperty("message")] public string Message { get; set; } = string.Empty;
    [JsonProperty("color")] public string Color { get; set; } = string.Empty;
    [JsonProperty("threadId")] public int ThreadId { get; set; }
    [JsonProperty("time")] public DateTime Time { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public dynamic LogMessage { get; set; } = default!;

    [NotMapped]
    [JsonProperty("message")]
    public string Message
    {
        get => LogMessage;
        set => LogMessage = value;
    }

    [System.Text.Json.Serialization.JsonIgnore]
    public LogEventLevel LogLevel { get; set; }

    [NotMapped]
    [JsonProperty("level")]
    public string Level
    {
        get => LogLevel.ToString();
        set => LogLevel = Enum.Parse<LogEventLevel>(value);
    }
}

public static class LogCache
{
    private static readonly Dictionary<string, List<LogEntry>?> Cache = new();

    public static bool TryGetCachedEntries(string filePath, out List<LogEntry>? cachedEntries)
    {
        return Cache.TryGetValue(filePath, out cachedEntries);
    }

    public static void AddToCache(string filePath, List<LogEntry>? entries)
    {
        Cache[filePath] = entries;
    }
}

public static class LogReader
{
    public static async Task<List<LogEntry>> GetLastDailyLogsAsync(string logDirectoryPath, int limit = 10,
        Func<LogEntry, bool>? filter = null)
    {
        IOrderedEnumerable<FileInfo> logFiles = GetLogFilesSortedByDate(logDirectoryPath);
        List<LogEntry> logEntries = new();

        IEnumerable<Task<IEnumerable<LogEntry>>> tasks =
            logFiles.Select(fileInfo => ProcessFileAsync(fileInfo.FullName, limit, filter));

        IEnumerable<LogEntry>[] results = await Task.WhenAll(tasks);

        foreach (IEnumerable<LogEntry>? entries in results)
        {
            logEntries.AddRange(entries ?? Array.Empty<LogEntry>());
            if (logEntries.Count >= limit) break;
        }

        return logEntries.Take(limit).ToList();
    }

    private static IOrderedEnumerable<FileInfo> GetLogFilesSortedByDate(string logDirectoryPath)
    {
        return Directory.GetFiles(logDirectoryPath, "*.txt")
            .Select(file => new FileInfo(file))
            .OrderByDescending(f => f.LastWriteTime);
    }

    private static async Task<IEnumerable<LogEntry>?> ProcessFileAsync(string filePath, int limit,
        Func<LogEntry, bool>? filter)
    {
        List<LogEntry> logEntries = new();
        FileInfo fileInfo = new(filePath);

        if (!fileInfo.Exists)
        {
            Logger.App($"File not found: {filePath}", LogEventLevel.Warning);
            return logEntries;
        }

        if (LogCache.TryGetCachedEntries(filePath, out List<LogEntry>? cachedEntries) && cachedEntries?.Count > 0 &&
            cachedEntries[0].Time >= fileInfo.LastWriteTime)
            return cachedEntries.Where(entry => filter == null || filter(entry)).Take(limit);

        await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader reader = new(fileStream);
        while (await reader.ReadLineAsync() is { } line && logEntries.Count < limit)
        {
            LogEntry? logEntry = line.FromJson<LogEntry>();
            if (logEntry != null && (filter == null || filter(logEntry))) logEntries.Add(logEntry);
        }

        LogCache.AddToCache(filePath, logEntries);
        return logEntries;
    }
}