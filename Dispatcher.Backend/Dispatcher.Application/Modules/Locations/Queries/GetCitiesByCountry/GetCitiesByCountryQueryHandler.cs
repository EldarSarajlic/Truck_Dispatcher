namespace Dispatcher.Application.Modules.Locations.Queries.GetCitiesByCountry;

public sealed class GetCitiesByCountryQueryHandler(IAppDbContext ctx)
    : IRequestHandler<GetCitiesByCountryQuery, List<GetCitiesByCountryQueryDto>>
{
    public async Task<List<GetCitiesByCountryQueryDto>> Handle(GetCitiesByCountryQuery request, CancellationToken ct)
        => await ctx.City
            .AsNoTracking()
            .Where(c => c.CountryId == request.CountryId)
            .OrderBy(c => c.Name)
            .Select(c => new GetCitiesByCountryQueryDto
            {
                Id   = c.Id,
                Name = c.Name,
            })
            .ToListAsync(ct);
}
