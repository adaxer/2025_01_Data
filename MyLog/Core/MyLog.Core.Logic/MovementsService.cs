using Microsoft.EntityFrameworkCore;
using MyLog.Data.DataAccess;
using MyLog.Data.Models;

namespace MyLog.Core.Logic;

public class MovementsService
{
    private readonly MyLogContext logContext;

    public MovementsService(MyLogContext logContext)
    {
        this.logContext = logContext;
    }

    public async Task<IEnumerable<Movement>> GetMovementsAsync(int count)
    {
        await logContext.Database.EnsureCreatedAsync();
        var result = await logContext.Movements.Take(count).ToListAsync();

        return result;
    }
}
