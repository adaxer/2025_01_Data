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
        modelBuilder.Entity<Address>().Property(a => a.Name).HasMaxLength(50);
        modelBuilder.Entity<Address>().Property(a => a.City).HasMaxLength(30);
        modelBuilder.Entity<Address>().Property(a => a.PostCode).HasMaxLength(10);
        base.OnModelCreating(modelBuilder);
    }
}
