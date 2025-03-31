namespace SimpleEventSourcing;
public sealed class EventSourcingConfiguration
{
    private static int _retryTimes;

    private static int _retryDelayMs;

    public int RetryTimes
    {
        get => _retryTimes;
        set => _retryTimes = value;
    }

    public int RetryDelayMs
    {
        get => _retryDelayMs;
        set => _retryDelayMs = value;
    }
}
