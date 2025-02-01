namespace Domain.PersonAggregate.DomainEvents;

/// <param name="newTaxCode"></param>
public sealed class PersonTaxCodeChanged(string newTaxCode)
{
    public string NewTaxCode { get; } = newTaxCode;
}
