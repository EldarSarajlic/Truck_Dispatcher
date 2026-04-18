namespace Dispatcher.Application.Modules.Locations.Queries.GetCountries;

public sealed class GetCountriesQueryDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string CountryCode { get; init; }
    public string PhoneCode { get; init; }
}
