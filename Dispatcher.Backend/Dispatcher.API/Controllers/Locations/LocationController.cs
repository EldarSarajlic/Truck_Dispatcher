using Dispatcher.Application.Modules.Locations.Queries.GetCitiesByCountry;
using Dispatcher.Application.Modules.Locations.Queries.GetCountries;

[ApiController]
[Route("api/locations")]
[Authorize]
public sealed class LocationController(ISender sender) : ControllerBase
{
    [HttpGet("countries")]
    public async Task<ActionResult<List<GetCountriesQueryDto>>> GetCountries(CancellationToken ct)
        => Ok(await sender.Send(new GetCountriesQuery(), ct));

    [HttpGet("countries/{countryId:int}/cities")]
    public async Task<ActionResult<List<GetCitiesByCountryQueryDto>>> GetCitiesByCountry(int countryId, CancellationToken ct)
        => Ok(await sender.Send(new GetCitiesByCountryQuery { CountryId = countryId }, ct));
}
