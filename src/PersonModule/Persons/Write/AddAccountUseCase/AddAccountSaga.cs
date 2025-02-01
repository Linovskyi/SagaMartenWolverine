using Persons.Contracts;
using Wolverine;

namespace Persons.Write.AddAccountUseCase;

/// <summary>
/// Add account Saga. 
/// </summary>
public class AddAccountSaga : Saga
{
    /// <summary>
    /// Saga Id.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Person Id.
    /// </summary>
    public string PersonId { get; set; }

    /// <summary>
    /// Account Name.
    /// </summary>
    public string AccountName { get; set; }

    /// <summary> 
    /// Start Saga handler.
    /// </summary>
    /// <param name="addAccountSagaStarted">Start Saga message</param>
    /// <returns></returns>
    public static (AddAccountSaga, AddAccountTimeoutExpired) Start(AddAccountSagaStarted addAccountSagaStarted) => (new AddAccountSaga
    {
        //Add data to the Saga state 
        Id = addAccountSagaStarted.AddAccountSagaId,
        PersonId = addAccountSagaStarted.PersonId,
        AccountName = addAccountSagaStarted.AccountName
    },
    new AddAccountTimeoutExpired(addAccountSagaStarted.AddAccountSagaId));

    /// <summary>
    /// Success Saga adding account
    /// </summary>
    /// <param name="_"></param>
    /// <param name="addAccountService">Add account service</param>
    public async void Handle(AccountApproved _, IAddAccountService addAccountService)
    {
        await addAccountService.CreateAccount(PersonId, AccountName);
        
        MarkCompleted();
    }

    /// <summary>
    /// Reject Saga handler
    /// MarkCompleted - closed Saga.
    /// </summary>
    /// <param name="_"></param>
    public void Handle(AccountRejected _) => MarkCompleted();

    /// <summary>
    /// </summary>
    /// <param name="_"></param>
    public void Handle(AddAccountTimeoutExpired _) => MarkCompleted();
}