using Domain.PersonAggregate.DomainEvents;
using Domain.PersonAggregate.Entities;
using Domain.PersonAggregate.Enums;
using Persons.Domain;
using Persons.Domain.DomainEvents;
using Shared.Abstractions;

namespace Domain.PersonAggregate;


public sealed class PersonAggregate : Aggregate
{

    public string Name { get; private set; }
    
    public string TaxCode { get; private set; }
    
    private readonly List<Account> _accounts = new List<Account>();
    
    public IReadOnlyCollection<Account> Accounts { get => _accounts.AsReadOnly(); }
    /// <param name="name"></param>
    /// <param name="taxCode"></param>
    public PersonAggregate(string name, string taxCode)
    {
        var @event = new PersonCreated(
            $"Person-{Guid.NewGuid()}",
            name,
            taxCode);

        Apply(@event);

        AddUncommittedEvent(@event);
    }

    private PersonAggregate()
    {
    }
    
    /// <param name="event"></param>
    protected void Apply(PersonCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        TaxCode = @event.Inn;
        Version++;
    }
    /// <param name="event"></param>
    protected void Apply(PersonNameChanged @event)
    {
        Name = @event.NewName;
        Version++;
    }

    protected void Apply(PersonTaxCodeChanged @event)
    {
        TaxCode = @event.NewTaxCode;
        Version++;
    }

    protected void Apply(AccountCreated @event)
    {
        var account = Account.Create(@event.AccountId, @event.Name);
        _accounts.Add(account);
        Version++;
    }

    protected void Apply(AccountAlreadyExists @event)
    {
        Version++;
    }

    protected void Apply(PaymentCreated @event)
    {
        var payment = Payment.Create(@event.Id, @event.Sum, @event.PaymentType);

        var account = _accounts.FirstOrDefault(x => x.Id == @event.AccountId);
        account.AddPayment(payment);
        Version++;
    }

    protected void Apply(BalanceBelowZeroPaymentRejected @event)
    {
        Version++;
    }
    
    /// <param name="newName"></param>
    /// <exception cref="ArgumentException"></exception>
    public DomainActionResult SetName(string newName)
    {
        var @event = new PersonNameChanged(newName);

        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }
    
    /// <param name="newTaxCode"></param>
    /// <exception cref="ArgumentException"></exception>
    public DomainActionResult SetTaxCode(string newTaxCode)
    {
        var @event = new PersonTaxCodeChanged(newTaxCode);

        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }
    
    /// <param name="accountName"></param>
    /// <exception cref="ArgumentException"></exception>
    public DomainActionResult AddAccount(string accountName)
    {
        if (Accounts.Any(x => x.Name.Equals(accountName)))
        {
            var accountExistsEvent = new AccountAlreadyExists(
            $"{nameof(Account)}-{Guid.NewGuid()}",
            Name = accountName);

            Apply(accountExistsEvent);
            AddUncommittedEvent(accountExistsEvent);

            return new DomainActionResult(DomainActionResultTypeEnum.Failed, "This account already exists");
        }

        var @event = new AccountCreated(
            $"{nameof(Account)}-{Guid.NewGuid()}",
            Name = accountName);

        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }
    
    /// <param name="accountId"></param>
    /// <param name="sum"></param>
    /// <param name="paymentType"></param>
    public DomainActionResult AddPayment(string accountId, decimal sum, PaymentTypeEnum paymentType)
    {
        var account = _accounts.First(x => x.Id == accountId);

        if (paymentType == PaymentTypeEnum.Debit && GetAccountSaldo(account) - sum < 0)
        {
            var @rejectionEvent = new BalanceBelowZeroPaymentRejected(
            $"{nameof(Payment)}-{Guid.NewGuid()}",
            accountId,
            sum,
            (int)paymentType);
            
            Apply(@rejectionEvent);
            AddUncommittedEvent(@rejectionEvent);

            return new DomainActionResult(DomainActionResultTypeEnum.Failed,"Balance below zero payment");
        }
        
        var @event = new PaymentCreated(
            $"{nameof(Payment)}-{Guid.NewGuid()}",
            accountId,
            sum,
            (int)paymentType);
        Apply(@event);
        AddUncommittedEvent(@event);

        return new DomainActionResult(DomainActionResultTypeEnum.Success);
    }
    
    /// <param name="account"></param>
    /// <returns></returns>
    private static decimal GetAccountSaldo(Account account)
    { 
        decimal saldo = 0;
        foreach(var payment in account.Payments) 
        {
            saldo = (payment.PaymentType == PaymentTypeEnum.Credit) ? saldo + payment.Sum : saldo - payment.Sum;
        }

        return saldo;
    }
}
