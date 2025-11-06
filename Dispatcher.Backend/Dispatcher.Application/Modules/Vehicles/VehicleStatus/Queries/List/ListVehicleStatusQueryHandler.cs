namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.List;

public sealed class ListVehicleStatusQueryHandler(IAppDbContext ctx):IRequestHandler<ListVehicleStatusQuery, PageResult<ListVehicleStatusQueryDto>>
{
public async Task<PageResult<ListVehicleStatusQueryDto>> Handle(ListVehicleStatusQuery request, CancellationToken cancellationToken)
    {
        var query = ctx.VehicleStatuses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(vs => vs.StatusName.Contains(request.Search));
        }

        var projectedQuery = query.OrderBy(x => x.StatusName).Select(vs => new ListVehicleStatusQueryDto
        {
            Id = vs.Id,
            StatusName = vs.StatusName,
            Description = vs.Description
        });
        return await PageResult<ListVehicleStatusQueryDto>.FromQueryableAsync(projectedQuery,request.Paging,cancellationToken);
    }
}