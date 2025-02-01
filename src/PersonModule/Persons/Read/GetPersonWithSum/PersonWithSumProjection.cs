using Domain.PersonAggregate.DomainEvents;
using Marten.Events.Aggregation;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Proection event aggregate PersonAggreate.
/// </summary>
public class PersonWithSumProjection : SingleStreamProjection<PersonWithSum>
{
    public PersonWithSumProjection()
    {
        ProjectEvent<PersonCreated>((item, @event) => item.Apply(@event));
        ProjectEvent<PersonTaxCodeChanged>((item, @event) => item.Apply(@event));
        ProjectEvent<PersonNameChanged>((item, @event) => item.Apply(@event));
        ProjectEvent<AccountCreated>((item, @event) => item.Apply(@event));
        ProjectEvent<PaymentCreated>((item, @event) => item.Apply(@event));
    }
}