using MyLog.Core.Contracts.Models;

namespace MyLog.Core.Contracts.Interfaces;

public interface IMovementsRepository
{
    Task<IEnumerable<MovementDto>> GetMovementsForUserAsync(int count, string userName);
}
