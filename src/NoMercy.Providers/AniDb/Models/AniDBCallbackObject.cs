#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using AniDB;
using AniDB.ResponseItems;

namespace NoMercy.Providers.AniDb.Models;

public class AniDbCallbackObject<T>(Action<T> callback) : IAniDBMessageResponseCallback<T>
    where T : IAniDBResponseItem
{
    public void Callback(T messageItem)
    {
        callback(messageItem);
    }
}