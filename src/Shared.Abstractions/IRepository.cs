using Shared.Abstractions;

namespace Shared.Infrastructure;

public interface IRepository
{
    /// <param name="aggregate"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task StoreAsync(Aggregate aggregate, CancellationToken ct = default);
    
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<T> LoadAsync<T>(string id, int? version = null, CancellationToken ct = default) where T : Aggregate;
}
