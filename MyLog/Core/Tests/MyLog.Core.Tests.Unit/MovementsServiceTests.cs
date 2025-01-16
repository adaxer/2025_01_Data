using MyLog.Core.Contracts.Interfaces;
using MyLog.Core.Contracts.Models;
using MyLog.Core.Logic;

namespace MyLog.Core.Tests.Unit;

public class MovementsServiceTests
{
    [Fact]
    public async Task GetMovementsAsync_Returns_Correct_CountAsync()
    {
        // Arrange
        IMovementsService movementsService = new MovementsService(new DummyRepository());

        // Act
        var result = await movementsService.GetMovementsAsync(5, "test");

        // Assert
        Assert.True(result.Count()==5);
        Assert.True(result.All(m => m.UserName == "test"));
    }
}

internal class DummyRepository : IMovementsRepository
{
    public DummyRepository()
    {
    }

    public Task<bool> DeleteMovementByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<MovementDetailDto?> GetMovementByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovementDto>> GetMovementsByUserAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MovementDto>> GetMovementsForUserAsync(int count, string userName)
    {
        return Task.FromResult(Enumerable.Range(0,10).Select(i=>new MovementDto() { UserName = userName }));
    }

    public Task<bool> UpdateMovementAsync(MovementDetailDto movementDetailDto)
    {
        throw new NotImplementedException();
    }
}
