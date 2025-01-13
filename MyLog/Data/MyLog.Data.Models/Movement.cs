using System.ComponentModel.DataAnnotations;

namespace MyLog.Data.Models;

public class Movement
{
    public int Id { get; set; } = 0;

    public string CargoNr { get; set; }
    public Address? Delivery { get; set; }
    public Address? PickUp { get; set; }
    public Address? CargoPayer { get; set; }

    public Movement(string cargoNr)
    {
        CargoNr = cargoNr;
    }
}
