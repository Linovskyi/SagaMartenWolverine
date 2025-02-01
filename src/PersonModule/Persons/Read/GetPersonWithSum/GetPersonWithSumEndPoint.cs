using Marten;
using Newtonsoft.Json;
using Persons.Contracts;
using Wolverine.Http;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Endpoint get persons.
/// </summary>
public static class GetPersonWithSumEndPoint
{
    /// <summary>
    /// get person by id.
    /// </summary>
    /// <param name="getPersonsWithSumCommand"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [WolverineGet("person/person")]
    public static async Task<string> Handle(GetPersonWithSumQuery getPersonsWithSumCommand, IQuerySession session)
    {
        var person = await session
            .Query<PersonWithSum>()
            .FirstOrDefaultAsync(c => c.Id == getPersonsWithSumCommand.PersonId) ?? throw new Exception($"Person not found.");

        return JsonConvert.SerializeObject(person, Formatting.Indented);
    }

    /// <summary>
    /// Get all person
    /// </summary>
    /// <param name="getPersonsWithSumCommand"></param>
    /// <param name="session"></param>
    /// <returns>Person with saldo</returns>
    /// <exception cref="Exception"></exception>
    [WolverineGet("person/persons")]
    public static async Task<string> Handle(GetPersonsWithSumQuery getPersonsWithSumCommand, IQuerySession session)
    {
        var persons = await session
            .Query<PersonWithSum>().ToListAsync() ?? throw new Exception($"Persons not found.");

        return JsonConvert.SerializeObject(persons, Formatting.Indented);
    }
}