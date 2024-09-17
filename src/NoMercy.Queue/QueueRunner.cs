using Microsoft.EntityFrameworkCore;
using NoMercy.Database;
using NoMercy.NmSystem;
using Serilog.Events;

namespace NoMercy.Queue;

public static class QueueRunner
{
    private static readonly
        Dictionary<string, (int count, List<Worker> workerInstances, CancellationTokenSource _cancellationTokenSource)>
        Workers = new()
        {
            ["queue"] = (1, [], new CancellationTokenSource()),
            ["encoder"] = (2, [], new CancellationTokenSource()),
            ["cron"] = (1, [], new CancellationTokenSource()),
            ["data"] = (10, [], new CancellationTokenSource()),
            ["image"] = (10, [], new CancellationTokenSource())
            // ["request"] = (15, [], new CancellationTokenSource()),
        };

    private static bool _isInitialized;
    private static readonly JobQueue JobQueue = new(new QueueContext());
    private static bool _isUpdating;

    public static async Task Initialize()
    {
        if (_isInitialized) return;

        // await Task.Delay(2000);

        _isInitialized = true;

        List<Task> taskList = [];

        await using QueueContext queueContext = new();
        await queueContext.QueueJobs
            .ForEachAsync(job => job.ReservedAt = null);
        await queueContext.SaveChangesAsync();

        foreach (KeyValuePair<string, (int count, List<Worker> workerInstances, CancellationTokenSource
                     _cancellationTokenSource)> keyValuePair in Workers)
            for (int i = 0; i < keyValuePair.Value.count; i++)
                taskList.Add(Task.Run(() => new Thread(() => SpawnWorker(keyValuePair.Key)).Start()));
        // await Task.Delay(2000);
        await Task.WhenAll(taskList);

        return;
    }

    private static Task SpawnWorker(string name)
    {
        Worker workerInstance = new(JobQueue, name);

        workerInstance.WorkCompleted += QueueWorkerCompleted(name, workerInstance);

        Workers[name].workerInstances.Add(workerInstance);

        workerInstance.Start();

        return Task.CompletedTask;
    }


    #region MyRegion

    public static Task Start(string name)
    {
        foreach (Worker workerInstance in Workers[name].workerInstances) workerInstance.Start();

        return Task.CompletedTask;
    }

    public static Task StartAll()
    {
        foreach (KeyValuePair<string, (int count, List<Worker> workerInstances, CancellationTokenSource
                     _cancellationTokenSource)> keyValuePair in Workers) Start(keyValuePair.Key);

        return Task.CompletedTask;
    }

    public static Task Stop(string name)
    {
        foreach (Worker workerInstance in Workers[name].workerInstances) workerInstance.Stop();

        return Task.CompletedTask;
    }

    public static Task StopAll()
    {
        foreach (KeyValuePair<string, (int count, List<Worker> workerInstances, CancellationTokenSource
                     _cancellationTokenSource)> keyValuePair in Workers) Stop(keyValuePair.Key);

        return Task.CompletedTask;
    }

    public static Task Restart(string name)
    {
        foreach (Worker workerInstance in Workers[name].workerInstances) workerInstance.Restart();

        return Task.CompletedTask;
    }

    public static Task RestartAll()
    {
        foreach (KeyValuePair<string, (int count, List<Worker> workerInstances, CancellationTokenSource
                     _cancellationTokenSource)> keyValuePair in Workers) Restart(keyValuePair.Key);

        return Task.CompletedTask;
    }

    #endregion


    private static WorkCompletedEventHandler QueueWorkerCompleted(string name, Worker instance)
    {
        return (sender, args) =>
        {
            if (!ShouldRemoveWorker(name)) return;

            instance.Stop();
            Workers[name].workerInstances.Remove(instance);
        };
    }

    private static bool ShouldRemoveWorker(string name)
    {
        return Workers[name].workerInstances.Count > Workers[name].count;
    }

    private static Task UpdateRunningWorkerCounts(string name)
    {
        if (ShouldRemoveWorker(name)) return Task.CompletedTask;

        int i = Workers[name].workerInstances.Count;

        Task.Run(async () =>
        {
            while (!_isUpdating && i < Workers[name].count)
            {
                if (_isUpdating || i >= Workers[name].count) break;

                Task.Run(() => SpawnWorker(name)).ConfigureAwait(ConfigureAwaitOptions.None).GetAwaiter();

                i += 1;

                await Task.Delay(100);
            }
        }, Workers[name]._cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public static async Task<bool> SetWorkerCount(string name, int max)
    {
        if (!Workers.ContainsKey(name)) return false;

        Logger.Queue($"Setting queue {name} to {max} workers", LogEventLevel.Information);
        _isUpdating = true;
        Workers[name]._cancellationTokenSource.Cancel();

        (int count, List<Worker> workerInstances, CancellationTokenSource _cancellationTokenSource) valueTuple =
            Workers[name];
        valueTuple.count = max;
        valueTuple._cancellationTokenSource = new CancellationTokenSource();
        Workers[name] = valueTuple;

        await Task.Run(() =>
        {
            _isUpdating = false;
            UpdateRunningWorkerCounts(name);
        }, Workers[name]._cancellationTokenSource.Token);

        return true;
    }

    public static int GetWorkerIndex(string name, Worker worker)
    {
        return Workers[name].workerInstances.IndexOf(worker);
    }
}