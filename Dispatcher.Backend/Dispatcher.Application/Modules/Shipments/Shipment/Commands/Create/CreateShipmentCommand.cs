using MediatR;

public class CreateShipmentCommand : IRequest<int>
{
    public decimal Weight { get; set; }
    public decimal Volume { get; set; }
    public string PickupLocation { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
}