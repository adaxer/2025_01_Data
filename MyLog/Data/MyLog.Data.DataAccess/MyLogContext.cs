using Microsoft.EntityFrameworkCore;
using MyLog.Data.Models;

namespace MyLog.Data.DataAccess;

public class MyLogContext : DbContext
{
    public DbSet<Movement> Movements { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public MyLogContext(DbContextOptions<MyLogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movement>().Property(m => m.CargoNr).HasMaxLength(15).IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}
