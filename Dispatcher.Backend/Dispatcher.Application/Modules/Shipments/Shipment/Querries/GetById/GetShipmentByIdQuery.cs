using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.GetById
{
    public class GetShipmentByIdQuery:IRequest<GetShipmentByIdQueryDto?>
    {
        public int Id { get; set; }
    }
}
