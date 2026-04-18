namespace Dispatcher.Application.Modules.Locations.Queries.GetCitiesByCountry;

public sealed class GetCitiesByCountryQuery : IRequest<List<GetCitiesByCountryQueryDto>>
{
    public int CountryId { get; init; }
}
