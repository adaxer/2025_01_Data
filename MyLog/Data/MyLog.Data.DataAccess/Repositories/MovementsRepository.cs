using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyLog.Core.Contracts.Interfaces;
using MyLog.Core.Contracts.Models;
using MyLog.Data.DataAccess.Mappings;
using MyLog.Data.Models;

namespace MyLog.Data.DataAccess.Repositories;

public class MovementsRepository : IMovementsRepository
{
    private readonly MyLogContext _context;
    private readonly ILogger<MovementsRepository> _logger;

    public MovementsRepository(MyLogContext context, ILogger<MovementsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<MovementDetailDto?> GetMovementByIdAsync(int id)
    {
        var dto = await _context.Movements.Include(m => m.CargoPayer).Include(m => m.Delivery).Include(m => m.PickUp).Where(m => m.Id == id).SingleOrDefaultAsync();
        return (dto == null)
            ? null
            : dto.ToDetailDto();
    }

    public async Task<IEnumerable<MovementDto>> GetMovementsForUserAsync(int count, string userName)
    {
        //var movements = await _context.Movements
        //    .Where(m => m.UserName==userName)
        //    .Take(count)
        //    .Include(m => m.Delivery)
        //    .ToListAsync();
        //return movements.Select(m=>m.ToDto());

        return await _context.MovementDtos.Where(m => m.UserName == userName)
            .Take(count)
            .ToListAsync();
    }

    public async Task<bool> UpdateMovementAsync(MovementDetailDto movementDetailDto)
    {
        try
        {
            var movement = await _context.Movements.FindAsync(movementDetailDto.Id);
            var pickup = await _context.Addresses.FindAsync(movementDetailDto.PickUpId);
            var delivery = await _context.Addresses.FindAsync(movementDetailDto.DeliveryId);
            var cargoPayer = await _context.Addresses.FindAsync(movementDetailDto.CargoPayerId);
            if (movement == null || pickup == null || delivery == null)
            {
                return false;
            }

            // Wenn die zusammenpassen, kann man CurrentValues mit einer Zeile setzen
            //var newmovement = new Movement(movementDetailDto.CargoNr) { CargoPayerId = movementDetailDto.CargoPayerId };
            //_context.Entry(movement).CurrentValues.SetValues(newmovement);

            movement.CargoPayer = cargoPayer;
            movement.CargoPayerId = movementDetailDto.CargoPayerId;
            movement.Delivery = delivery;
            movement.PickUp = pickup;
            movement.UserName = movementDetailDto.UserName; // todo: User checken!
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not save changes");
            return false;
        }
    }

    public async Task<bool> DeleteMovementByIdAsync(int id)
    {
        //var movement = await _context.Movements.FindAsync(id);
        //if (movement != null)
        //{
        //    _context.Remove(movement);
        //    await _context.SaveChangesAsync();
        //}
        //return (movement != null);

        //        var result = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Movements WHERE Id={id}");
        var result = await _context.Database.ExecuteSqlRawAsync("EXEC DeleteMovementById @Id", new SqlParameter("@Id", id));

        return (result > 0);
    }
}
