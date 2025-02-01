using Persons.Contracts;
using Wolverine;
using Wolverine.Attributes;
using Wolverine.Http;

namespace Persons.Write.CreatePersonUseCase;

/// <summary>
/// EndPoint create new person.
/// </summary>
[WolverineHandler]
public static class CreatePersonEndPoint
{
    [WolverinePost("person/start-create-person-saga")]
    public static async Task<CreatePersonSagaResponse> Handle(CreatePersonCommand createPersonCommand, IMessageBus bus)
    {
        var sagaId = $"CreatePersonSaga-{Guid.NewGuid()}";
        // Send event to wolverine with starting Saga Create new person.
        // Account was added if send message to Saga - PersonApproved.
        await bus.PublishAsync(
            new CreatePersonSagaStarted(
                sagaId,
                createPersonCommand.Name,
                createPersonCommand.Inn));

        return new CreatePersonSagaResponse(sagaId);
    }
}
