namespace AsyncProjectionStressTest.Common.Core;

public interface IAggregate
{
    Guid Id { get; }
    long Version { get; }
    void ClearEvents();
    IReadOnlyCollection<object> GetEvents();
}