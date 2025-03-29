namespace SimpleEventSourcing.Singletons;
internal sealed class EventSourcingConfiguration
{
    private static EventSourcingConfiguration? _instance;

    public static EventSourcingConfiguration Instance => _instance ??= new EventSourcingConfiguration();

    public int RetryTimes { get; set; }

    public int RetryDelayMs { get; set; }
}
