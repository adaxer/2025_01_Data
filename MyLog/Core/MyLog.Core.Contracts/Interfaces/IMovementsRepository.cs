using MyLog.Core.Contracts.Models;

namespace MyLog.Core.Contracts.Interfaces;

public interface IMovementsRepository
{
    Task<IEnumerable<MovementDto>> GetMovementsForUserAsync(int count, string userName);
    Task<MovementDetailDto?> GetMovementByIdAsync(int id);
    Task<bool> UpdateMovementAsync(MovementDetailDto movementDetailDto);
    Task<bool> DeleteMovementByIdAsync(int id);
}
