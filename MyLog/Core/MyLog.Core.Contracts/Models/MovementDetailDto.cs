namespace MyLog.Core.Contracts.Models;

public class MovementDetailDto
{
    public int Id { get; set; }

    public string CargoNr { get; set; } = string.Empty;
    public int? DeliveryId { get; set; }
    public int? PickUpId { get; set; }
    public int? CargoPayerId { get; set; }
    public string UserName { get; set; } = string.Empty;
}
