using Domain.PersonAggregate.Enums;
using JasperFx.Core;
using Wolverine;

namespace Persons.Contracts;

/// <param name="PersonId">Person id.</param>
public record GetPersonWithSumQuery(string PersonId);

public record GetPersonsWithSumQuery();

/// <param name="PersonId">Person id.</param>
/// <param name="AccountName">Account name</param>
public record AddAccountCommand(string PersonId, string AccountName);

/// <param name="SagaId">Saga Id</param>
public record AddAccountSagaResponse(string SagaId);

/// <param name="PersonId"></param>
/// <param name="AccountId"></param>
/// <param name="Sum"></param>
/// <param name="PaymentType"></param>
public record AddPaymentCommand(string PersonId, string AccountId, decimal Sum, PaymentTypeEnum PaymentType);

/// <param name="SagaId"></param>
public record AddPaymentSagaResponse(string SagaId);

/// <param name="Name"></param>
/// <param name="Inn"></param>
public record CreatePersonCommand(string Name, string Inn);
/// <param name="SagaId"></param>
public record CreatePersonSagaResponse(string SagaId);
public record CreatePersonSagaStarted(string CreatePersonSagaId, string Name, string Inn);
public record PersonApproved(string Id);
public record PersonRejected(string Id);
/// <param name="Id"></param>
public record PersonCreationTimeoutExpired(string Id) : TimeoutMessage(3.Minutes());
/// <param name="AddPaymentSagaId"></param>
/// <param name="PersonId"></param>
/// <param name="AccountId"></param>
/// <param name="Sum"></param>
/// <param name="PaymentType"></param>
public record AddPaymentSagaStarted(string AddPaymentSagaId, string PersonId, string AccountId, decimal Sum, PaymentTypeEnum PaymentType);
/// <param name="Id"></param>
public record PaymentApproved(string Id);
/// <param name="Id"></param>
public record PaymentRejected(string Id);
/// <param name="Id"></param>
public record AddPaymentTimeoutExpired(string Id) : TimeoutMessage(3.Minutes());
/// <param name="AddAccountSagaId"></param>
/// <param name="PersonId"></param>
/// <param name="AccountName"></param>
public record AddAccountSagaStarted(string AddAccountSagaId, string PersonId, string AccountName);
/// <param name="Id"></param>
public record AccountApproved(string Id);
/// <param name="Id"></param>
public record AccountRejected(string Id);
/// <param name="Id"></param>
public record AddAccountTimeoutExpired(string Id) : TimeoutMessage(3.Minutes());