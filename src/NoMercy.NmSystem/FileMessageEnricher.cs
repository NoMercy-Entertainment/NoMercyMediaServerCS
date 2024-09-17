using Serilog.Core;
using Serilog.Events;

namespace NoMercy.NmSystem;
internal class FileMessageEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.RemovePropertyIfPresent("@mt");
    }
}