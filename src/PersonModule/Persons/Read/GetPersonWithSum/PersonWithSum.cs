using Domain.PersonAggregate.DomainEvents;
using Domain.PersonAggregate.Enums;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Person model using in proection PersonWithSumProjection.
/// </summary>
public class PersonWithSum
{
    /// <summary>
    /// Person Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tax code
    /// </summary>
    public string TaxCode { get; set; }

    /// <summary>
    /// Saldo.
    /// </summary>
    public decimal Saldo { get; set; }

    /// <summary>
    /// Update version after any event.
    /// </summary>
    public long Version { get; private set; }

    /// <summary>
    /// Accounts.
    /// </summary>
    public List<Account> Accounts = new List<Account>();

    /// <summary>
    /// Method Apply.
    /// </summary>
    /// <param name="event"></param>
    public void Apply(PersonCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        TaxCode = @event.Inn;
        Version++;
    }

    public void Apply(PersonNameChanged @event)
    {
        Name = @event.NewName;
        Version++;
    }

    public void Apply(PersonTaxCodeChanged @event)
    {
        TaxCode = @event.NewTaxCode;
        Version++;
    }

    public void Apply(AccountCreated @event)
    {
        var account = new Account(@event.AccountId, @event.Name, new List<Payment>());
        Accounts.Add(account);
        Version++;
    }

    public void Apply(PaymentCreated @event)
    {
        var payment = new Payment(@event.Id, @event.Sum, @event.PaymentType);
        var account = Accounts.FirstOrDefault(x => x.Id == @event.AccountId) ?? throw new ArgumentNullException($"Счёт не найден с ид {@event.AccountId}");
        account.Payments.Add(payment);

        Saldo = @event.PaymentType == (int)PaymentTypeEnum.Credit ? Saldo + @event.Sum : Saldo - @event.Sum;

        Version++;
    }
}

/// <summary>
/// Payment.
/// </summary>
public record Payment(string Id, decimal Sum, int PaymentType);

/// <summary>
/// Account.
/// </summary>
public record Account(string Id, string Name, List<Payment> Payments);
