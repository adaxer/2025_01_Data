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

    public async Task<IEnumerable<Movement>> GetMovementsAsync(int count, string userName)
    {
        await logContext.Database.EnsureCreatedAsync();
        var result = await logContext.Movements.Where(m=>m.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).Take(count).ToListAsync();

        return result;
    }
}
