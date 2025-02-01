using Domain.PersonAggregate.Enums;
using Persons.Contracts;
using Wolverine;

namespace Persons.Write.AddPaymentUseCase;

public class AddPaymentSaga : Saga
{
    public string? Id { get; set; }

    public string PersonId { get; set; }

    public string AccountId { get; set; }

    public decimal Sum { get; set; }

    public PaymentTypeEnum PaymentType { get; set; }

    public static (AddPaymentSaga, AddPaymentTimeoutExpired) Start(AddPaymentSagaStarted addPaymentSagaStarted) => (new AddPaymentSaga
    {
        Id = addPaymentSagaStarted.AddPaymentSagaId,
        PersonId = addPaymentSagaStarted.PersonId,
        AccountId = addPaymentSagaStarted.AccountId,
        Sum = addPaymentSagaStarted.Sum,
        PaymentType = addPaymentSagaStarted.PaymentType,
    },
    new AddPaymentTimeoutExpired(addPaymentSagaStarted.AddPaymentSagaId));

    public void Handle(PaymentApproved _, IAddPaymentService addPaymentService)
    {
        addPaymentService.CreatePayment(PersonId, AccountId, Sum, PaymentType);
        MarkCompleted();
    }

    /// <summary>
    /// Reject Saga finish
    /// </summary>
    /// <param name="_"></param>
    public void Handle(PaymentRejected _) => MarkCompleted();

    /// <summary>
    /// Reject Saga after timeout
    /// </summary>
    /// <param name="_"></param>
    public void Handle(AddPaymentTimeoutExpired _) => MarkCompleted();
}