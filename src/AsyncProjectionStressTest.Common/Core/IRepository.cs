namespace AsyncProjectionStressTest.Common.Core;

public interface IRepository<T>
    where T : class, IAggregate
{
    Task<T?> Find(Guid id, long? version = null, CancellationToken cancellationToken = default);
    Task Store(T aggregate, CancellationToken cancellationToken);
}