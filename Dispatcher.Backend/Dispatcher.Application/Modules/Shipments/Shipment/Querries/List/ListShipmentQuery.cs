using MediatR;
using System.Collections.Generic;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.List
{
    public class ListShipmentQuery : IRequest<List<ListShipmentQueryDto>>
    {
        // dodati filtere po potrebi, npr. status, pickup location itd.
        public string? Status { get; set; }
        public string? PickupLocation { get; set; }
    }
}