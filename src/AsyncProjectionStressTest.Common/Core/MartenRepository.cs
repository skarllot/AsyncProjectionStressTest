using Marten;

namespace AsyncProjectionStressTest.Common.Core;

public class MartenRepository<T> : IRepository<T>
    where T : class, IAggregate
{
    private readonly IDocumentStore _documentStore;

    public MartenRepository(IDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    public async Task<T?> Find(Guid id, long? version = null, CancellationToken cancellationToken = default)
    {
        var session = _documentStore.LightweightSession();
        await using (session.ConfigureAwait(false))
        {
            var aggregate = await session.Events
                .AggregateStreamAsync<T>(id, version ?? 0, token: cancellationToken)
                .ConfigureAwait(false);

            aggregate?.ClearEvents();
            return aggregate;
        }
    }

    public async Task Store(T aggregate, CancellationToken cancellationToken)
    {
        var events = aggregate.GetEvents();
        if (events.Count == 0)
            return;

        var session = _documentStore.LightweightSession();
        await using (session.ConfigureAwait(false))
        {
            if (events.Count == aggregate.Version)
                session.Events.StartStream(aggregate.GetType(), aggregate.Id, events);
            else
                session.Events.Append(aggregate.Id, aggregate.Version, events);
            
            await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        
        aggregate.ClearEvents();
    }
}