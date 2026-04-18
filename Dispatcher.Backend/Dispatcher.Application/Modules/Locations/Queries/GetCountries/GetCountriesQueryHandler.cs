namespace Dispatcher.Application.Modules.Locations.Queries.GetCountries;

public sealed class GetCountriesQueryHandler(IAppDbContext ctx)
    : IRequestHandler<GetCountriesQuery, List<GetCountriesQueryDto>>
{
    public async Task<List<GetCountriesQueryDto>> Handle(GetCountriesQuery request, CancellationToken ct)
        => await ctx.Country
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new GetCountriesQueryDto
            {
                Id          = c.Id,
                Name        = c.Name,
                CountryCode = c.CountryCode,
                PhoneCode   = c.PhoneCode,
            })
            .ToListAsync(ct);
}
