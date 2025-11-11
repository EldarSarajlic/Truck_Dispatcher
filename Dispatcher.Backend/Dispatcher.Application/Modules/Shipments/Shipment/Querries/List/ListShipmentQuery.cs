using MediatR;
using System.Collections.Generic;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.List
{
    public sealed class ListShipmentQuery : BasePagedQuery<ListShipmentQueryDto>
    {
        /// <summary>
        /// Filter by shipment status (e.g., "Pending", "ReadyForDispatch", "InTransit")
        /// </summary>
        public string? Status { get; init; }

        /// <summary>
        /// Filter by client's first or last name
        /// </summary>
        public string? ClientName { get; init; }

        /// <summary>
        /// Filter by delivery city name
        /// </summary>
        public string? DeliveryCityName { get; init; }
    }
}