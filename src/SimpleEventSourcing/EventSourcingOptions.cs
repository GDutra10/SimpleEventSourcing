namespace SimpleEventSourcing;

public class EventSourcingOptions
{
    /// <summary>
    /// Should use InMemory
    /// </summary>
    public bool UseInMemory { get; set; }

    /// <summary>
    /// How many times should retry. Default value is always 0 even if set to less than this. 
    /// </summary>
    public int RetryTimes { get; set; } = 0;

    /// <summary>
    /// Delay to retry in MS
    /// </summary>
    public int RetryDelayMs { get; set; } = 200;
}
