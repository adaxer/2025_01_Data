
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyLog.Data.Models;

namespace MyLog.Data.DataAccess;

public class MyLogInitializer
{
    private readonly IServiceProvider _services;

    public MyLogInitializer(IServiceProvider services)
    {
        _services = services;
    }

    public async Task SeedTestDataAsync()
    {
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyLogContext>();

        var movement = context.Movements.FirstOrDefault();
        if(movement != null)
        {
            return;
        }

        var testAddressFaker = new Faker<Address>()
        .CustomInstantiator(f => new Address(
            f.Name.LastName(),
            f.Address.City(),
            f.Address.StreetAddress(),
            f.Address.ZipCode()))
        .RuleFor(a => a.Name, (f,a) => f.Name.LastName())
        .RuleFor(a => a.City, f => f.Address.City())
        .RuleFor(a => a.Street, f => f.Address.StreetAddress())
        .RuleFor(a => a.PostCode, f => f.Address.ZipCode());
        
        var addresses = testAddressFaker.Generate(5);
        context.Addresses.AddRange(addresses);
        await context.SaveChangesAsync();
        var addressIds = addresses.Select(a => a.Id).ToList();

        var index = 1;
        var userNames = new List<string> { "bob", "alice", "Maier" };
        var testMovementFaker = new Faker<Movement>()
            .CustomInstantiator(f => new Movement($"1337{index++.ToString().PadLeft(3, '0')}"))
            .RuleFor(m=>m.UserName, f=>f.PickRandom(userNames))
            .RuleFor(m => m.CargoPayerId, f => f.PickRandom(addressIds))
            .RuleFor(m => m.PickUp, f => f.PickRandom(addresses))
            .RuleFor(m => m.Delivery, f => f.PickRandom(addresses));

        var movements = testMovementFaker.Generate(20);
        context.Movements.AddRange(movements);
        
        await context.SaveChangesAsync();
    }
}
