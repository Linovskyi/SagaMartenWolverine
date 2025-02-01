using Marten;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Persons.Contracts;
using Wolverine;

namespace Persons.Write.Subscriptions;

//Integration class event to send into Kafka
public record PersonApprovedIntegrationEvent(string Key, string Value);

/// <summary>
/// Subscribe PersonApproved type
/// </summary>
public class PersonApprovedToKafkaSubscription : SubscriptionBase
{
    private readonly IServiceProvider _serviceProvider;

    public PersonApprovedToKafkaSubscription(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        SubscriptionName = nameof(PersonApprovedToKafkaSubscription);
        
        IncludeType<PersonApproved>();
        
        Options.BatchSize = 1000;
        Options.MaximumHopperSize = 10000;
        
        Options.SubscribeFromPresent();
    }
    
    public override async Task<IChangeListener> ProcessEventsAsync(
        EventRange page,
        ISubscriptionController controller,
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        // с помощью Woverine будем отправлять интеграционные события в кафку.
        var messageBus = _serviceProvider.GetService<IMessageBus>() ?? throw new ArgumentNullException("Шина событий не зарегистрирована в IoC");

        foreach (var @event in page.Events)
        {
            await messageBus.PublishAsync(
                new PersonApprovedIntegrationEvent(@event.Data.GetType().Name, JsonConvert.SerializeObject(@event.Data)));
        }

        return NullChangeListener.Instance;
    }
}