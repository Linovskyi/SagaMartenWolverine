using Domain.PersonAggregate;

namespace Persons.Write.CreatePersonUseCase;

/// <summary>
/// Create person service.
/// </summary>
public interface ICreatePersonService
{
    /// <summary>
    /// Create person
    /// </summary>
    /// <param name="name"></param>
    /// <param name="inn"></param>
    /// <returns></returns>
    Task<PersonAggregate> Create(string name, string inn);

    /// <summary>
    /// Get person sate.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    Task<PersonAggregate> Get(string personId);

    /// <summary>
    /// Edit tax code.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="taxCode"></param>
    /// <returns></returns>
    Task<PersonAggregate> ChangeTaxCode(string personId, string taxCode);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<PersonAggregate> ChangeName(string personId, string name);
}
