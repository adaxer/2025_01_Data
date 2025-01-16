using Microsoft.EntityFrameworkCore;
using MyLog.Core.Contracts.Models;
using MyLog.Data.Models;

namespace MyLog.Data.DataAccess;

public class MyLogContext : DbContext
{
    public DbSet<Movement> Movements { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<MovementDto> MovementDtos { get; set; }

    public MyLogContext(DbContextOptions<MyLogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
      
        modelBuilder.Entity<Movement>().Property(m => m.CargoNr).HasMaxLength(15).IsRequired();
        modelBuilder.Entity<Movement>().HasIndex(m=>m.UserName).HasDatabaseName("IX_Movement_UserName");

        modelBuilder.Entity<Address>().Property(a => a.Name).HasMaxLength(50);
        modelBuilder.Entity<Address>().Property(a => a.City).HasMaxLength(30);
        modelBuilder.Entity<Address>().Property(a => a.PostCode).HasMaxLength(10);

        modelBuilder.Entity<MovementDto>()
               .HasNoKey()
               .ToView("MovementDtos")
               .Property(v => v.Id).HasColumnName("Id");
    }
}
