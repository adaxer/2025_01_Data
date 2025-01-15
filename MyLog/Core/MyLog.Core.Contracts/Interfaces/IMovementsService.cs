using MyLog.Core.Contracts.Models;

namespace MyLog.Core.Contracts.Interfaces;

public interface IMovementsService
{
    Task<IEnumerable<MovementDto>> GetMovementsAsync(int count, string userName);
}
