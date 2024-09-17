// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using NoMercy.Queue;

namespace NoMercy.MediaProcessing.Jobs.PaletteJobs;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Serializable]
public abstract class AbstractPaletteJob<T> : IShouldQueue
{
    public int Id { get; set; }

    public abstract string QueueName { get; }
    public abstract int Priority { get; }

    public abstract Task Handle();

    public T[] Storage { get; set; } = [];

    // If the disposability is needed, do this =>
    // private T[]? _storage;
    // public IEnumerable<T> Storage {
    //     get => _storage ??= [];
    //     set => _storage = value.ToArray();
    // }

    #region IDisposable Support

    private void ReleaseUnmanagedResources()
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        GC.WaitForPendingFinalizers();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
    }

    #endregion
}