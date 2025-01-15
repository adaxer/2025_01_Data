﻿using Microsoft.EntityFrameworkCore;
using MyLog.Core.Contracts.Interfaces;
using MyLog.Core.Contracts.Models;
using MyLog.Data.DataAccess.Mappings;

namespace MyLog.Data.DataAccess.Repositories;

public class MovementsRepository : IMovementsRepository
{
    private readonly MyLogContext _context;

    public MovementsRepository(MyLogContext context)
    {
        _context = context;
    }

    public async Task<MovementDetailDto?> GetMovementByIdAsync(int id)
    {
        var dto = await _context.Movements.Include(m=>m.CargoPayer).Include(m=>m.PickUp).Where(m=>m.Id == id).SingleOrDefaultAsync();
        return (dto==null) 
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

    public Task<bool> UpdateMovementAsync(MovementDetailDto movementDetailDto)
    {
        throw new NotImplementedException();
    }
}
