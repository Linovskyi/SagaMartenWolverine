using Persons.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Persons.Write.AddAccountUseCase;

/// <summary>
/// New account person EndPoint.
/// </summary>
[WolverineHandler]
public static class AddAccountEndPoints
{
    [WolverinePost("person/start-add-account-saga")]
    public static async Task<AddAccountSagaResponse> Handle(AddAccountCommand addAccountCommand, IMessageBus bus)
    {
        var sagaId = $"AddAccountSaga-{Guid.NewGuid()}";
        // Send to wolverine event adding new account with starting Saga.
        // Account will create if send message to the SagaAccountApproved.
        await bus.PublishAsync(
            new AddAccountSagaStarted(
                sagaId,
                addAccountCommand.PersonId,
                addAccountCommand.AccountName));

        return new AddAccountSagaResponse(sagaId);
    }
}
