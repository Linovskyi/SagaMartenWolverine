using Persons.Contracts;
using Wolverine;

namespace Persons.Write.CreatePersonUseCase;

public class CreatePersonSaga : Saga
{
    public string? Id { get; set; }

    public string Name { get; set; }

    public string Inn { get; set; }

    public static (CreatePersonSaga, PersonCreationTimeoutExpired) Start(CreatePersonSagaStarted createPersonSagaStarted) => (new CreatePersonSaga
    {
        Id = createPersonSagaStarted.CreatePersonSagaId,
        Name = createPersonSagaStarted.Name,
        Inn = createPersonSagaStarted.Inn,
    },
    new PersonCreationTimeoutExpired(createPersonSagaStarted.CreatePersonSagaId));

    /// <summary>
    /// Success Saga handler
    /// </summary>
    /// <param name="_"></param>
    /// <param name="createPersonService"></param>
    public async void Handle(PersonApproved _, ICreatePersonService createPersonService)
    {
        //Save Person
        await createPersonService.Create(Name, Inn);
        //Closed Saga
        MarkCompleted();
    }

    /// <summary>
    /// Reject Saga handler
    /// </summary>
    /// <param name="_"></param>
    public async void Handle(PersonRejected _) => MarkCompleted();

    /// <summary>
    /// Reject Saga handler after timeout
    /// </summary>
    /// <param name="_"></param>
    public void Handle(PersonCreationTimeoutExpired _) => MarkCompleted();
}