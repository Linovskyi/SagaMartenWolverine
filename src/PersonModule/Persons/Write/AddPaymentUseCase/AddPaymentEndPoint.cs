using Persons.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Persons.Write.AddPaymentUseCase;

[WolverineHandler]
public static class AddPaymentEndPoint
{
    [WolverinePost("person/start-add-payment-saga")]
    public static async Task<AddPaymentSagaResponse> Handle(AddPaymentCommand addPaymentCommand, IMessageBus bus)
    {
        var sagaId = $"AddPaymentSaga-{Guid.NewGuid()}";
        await bus.PublishAsync(
            new AddPaymentSagaStarted(
                sagaId,
                addPaymentCommand.PersonId,
                addPaymentCommand.AccountId,
                addPaymentCommand.Sum,
                addPaymentCommand.PaymentType));

        return new AddPaymentSagaResponse(sagaId);
    }
}
