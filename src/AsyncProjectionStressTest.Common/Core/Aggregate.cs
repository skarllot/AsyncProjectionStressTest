namespace AsyncProjectionStressTest.Common.Core;

public abstract class Aggregate : IAggregate
{
    [NonSerialized] private readonly List<object> _pendingEvents = new();

    protected Aggregate(Guid id) => Id = id;

    public Guid Id { get; protected set; }

    public long Version { get; private set; }

    void IAggregate.ClearEvents() => _pendingEvents.Clear();

    IReadOnlyCollection<object> IAggregate.GetEvents() => _pendingEvents;

    protected void OnEventApplied(object @event)
    {
        _pendingEvents.Add(@event);
        Version++;
    }
}