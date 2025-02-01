using Persons.Contracts;
using SagaApprovement.Contracts;
using Wolverine;
using Wolverine.Http;

namespace Application.SagaApprovmentEndPoints;

public static class ApproveEndPoints
{
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("approve-add-account-saga")]
    public static ValueTask Handle(ApproveAccountCommand command, IMessageBus bus) => bus.PublishAsync(new AccountApproved(command.SagaId));
    
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("approve-add-payment-saga")]
    public static ValueTask Handle(ApprovePaymentCommand command, IMessageBus bus) => bus.PublishAsync(new PaymentApproved(command.SagaId));
    
    
    /// <param name="command"></param>
    /// <param name="bus"></param>
    /// <returns></returns>
    [WolverinePost("approve-person-creation-saga")]
    public static ValueTask Handle(ApprovePersonCreationCommand command, IMessageBus bus) => bus.PublishAsync(new PersonApproved(command.SagaId));
}
