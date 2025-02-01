using Marten;
using Shared.Abstractions;
using Shared.Infrastructure;

namespace Shared;

/// <inheritdoc/>
public sealed class Repository(IDocumentStore store) : IRepository
{
    //Marten document store
    private readonly IDocumentStore store = store;
    public async Task StoreAsync(Aggregate aggregate, CancellationToken ct = default)
    {
        await using var session = await store.LightweightSerializableSessionAsync(token: ct);
        
        var events = aggregate.GetUncommittedEvents().ToArray();
        
        session.Events.Append(aggregate.Id, aggregate.Version, events);
        
        await session.SaveChangesAsync(ct);
        
        aggregate.ClearUncommittedEvents();
    }
    
    public async Task<T> LoadAsync<T>(
        string id,
        int? version = null,
        CancellationToken ct = default
    ) where T : Aggregate
    {
        await using var session = await store.LightweightSerializableSessionAsync(token: ct);
        
        var stream = await session.Events.FetchForWriting<T>(id);

        return stream.Aggregate;        
    }
}