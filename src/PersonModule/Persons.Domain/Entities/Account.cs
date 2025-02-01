using Shared.Abstractions;

namespace Domain.PersonAggregate.Entities;

public class Account : Entity
{
    private readonly List<Payment> _payments = new List<Payment>();

    protected Account()
    {
    }
    
    public string Name { get; private set; }
    
    public IReadOnlyCollection<Payment> Payments { get => _payments.AsReadOnly(); }

    internal protected static Account Create(string accountId, string name)
    {
        var account = new Account()
        {
            Id = accountId,
            Name = name,
        };

        return account;
    }

    internal protected void AddPayment(Payment payment) => _payments.Add(payment);
}
