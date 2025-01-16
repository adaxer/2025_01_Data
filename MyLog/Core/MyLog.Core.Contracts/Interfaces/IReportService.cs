using MyLog.Core.Contracts.Models;

namespace MyLog.Core.Contracts;

public interface IReportService
{
    Task CreateMovementReportAsync(List<MovementDto> data);
}
