using Marten;
using Marten.Events.Projections;

namespace Persons.Read.GetPersonWithSum;

/// <summary>
/// Register proection in Marten.
/// </summary>
public static class ProjectionsConfiguration
{
    internal static void ConfigureProjections(this StoreOptions options)
    {
        options.Projections.Add<PersonWithSumProjection>(ProjectionLifecycle.Async);
    }
}