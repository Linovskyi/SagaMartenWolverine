using Persons.Contracts;
using SagaRejection.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Application.SagaRejectionEndPoints;

[WolverineHandler]
public static class RejectEndPoints
{
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("reject-add-account-saga")]
    public static ValueTask Handle(RejectAccountCommand command, IMessageBus bus) => bus.PublishAsync(new AccountRejected(command.SagaId));
    
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("reject-add-payment-saga")]
    public static ValueTask Handle(RejectPaymentCommand command, IMessageBus bus) => bus.PublishAsync(new PaymentRejected(command.SagaId));
    
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("reject-person-creation-saga")]
    public static ValueTask Handle(RejectPersonCreationCommand command, IMessageBus bus) => bus.PublishAsync(new PersonRejected(command.SagaId));
}
