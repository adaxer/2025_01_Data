﻿using MyLog.Core.Contracts.Models;
using MyLog.Data.Models;

namespace MyLog.Data.DataAccess.Mappings;

public static class MovementMappings
{
    public static MovementDto ToDto(this Movement movement)
    {
        return new MovementDto
        {
            Id = movement.Id,
            CargoNr = movement.CargoNr,
            Delivery = $"{movement?.Delivery?.Name}{Environment.NewLine}{movement?.Delivery?.PostCode} {movement?.Delivery?.City}",
            PickUpId = movement!.PickupId,
            CargoPayerId = movement.CargoPayerId,
            UserName = movement.UserName
        };
    }
}
