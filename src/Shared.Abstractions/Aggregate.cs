using System.Text.Json.Serialization;

namespace Shared.Abstractions;
public abstract class Aggregate : Entity
{
    public long Version { get; set; }
    
    [JsonIgnore] private readonly List<object> _uncommittedEvents = new List<object>();
    
    public IEnumerable<object> GetUncommittedEvents()
    {
        return _uncommittedEvents;
    }
    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }
    
    /// <param name="event"></param>
    protected void AddUncommittedEvent(object @event)
    {
        _uncommittedEvents.Add(@event);
    }
}
