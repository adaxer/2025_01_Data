using MyLog.Core.Contracts.Interfaces;
using MyLog.Core.Contracts.Models;

namespace MyLog.Core.Logic;

public class MovementsService : IMovementsService
{
    private readonly IMovementsRepository _repository;

    public MovementsService(IMovementsRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> DeleteMovementByIdAsync(int id) => _repository.DeleteMovementByIdAsync(id);

    public Task<MovementDetailDto?> GetMovementByIdAsync(int id)=> _repository.GetMovementByIdAsync(id);

    public async Task<IEnumerable<MovementDto>> GetMovementsAsync(int count, string userName)
    {
        var movements = await _repository.GetMovementsForUserAsync(count, userName);
        return movements.Take(count).ToList();
    }

    public async Task<bool> UpdateMovementAsync(MovementDetailDto movementDetailDto)
    {
        var success = await _repository.UpdateMovementAsync(movementDetailDto);
        return success;
    }
}
