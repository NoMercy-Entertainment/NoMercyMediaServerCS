#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using NoMercy.Database;

namespace NoMercy.Server.app.Helper;

public static class Databases
{
    static internal QueueContext QueueContext { get; set; }
    static internal MediaContext MediaContext { get; set; }
}