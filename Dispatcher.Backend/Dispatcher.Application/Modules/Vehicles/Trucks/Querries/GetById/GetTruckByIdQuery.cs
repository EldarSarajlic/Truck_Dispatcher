using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Querries.GetById
{
    public class GetTruckByIdQuery:IRequest<GetTruckByIdQueryDto?>
    {
        public int Id { get; set; }
    }
}
