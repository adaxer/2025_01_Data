namespace MyLog.Core.Contracts.Models;

public class MovementDto
{
    public int Id { get; set; }

    public string CargoNr { get; set; } = string.Empty;
    public string Delivery { get; set; } = string.Empty;
    public int PickUpId { get; set; }
    public int CargoPayerId { get; set; }

    public string UserName { get; set; } = string.Empty;
}
