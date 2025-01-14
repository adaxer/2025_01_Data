using ADaxer.Auth;
using ADaxer.Auth.Endpoints;
using ADaxer.Auth.User;

namespace SecureApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddConfiguredUserDb(o =>
        {
            o.UserDbConnectionString = builder.Configuration.GetConnectionString("AppDb")!;
            o.EncryptionSecret = builder.Configuration.GetValue<string>("Jwt:Secret")!;
            o.TokenIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer")!;
            o.TokenAudience = builder.Configuration.GetValue<string>("Jwt:Audience")!;
            o.TokenLifetime = builder.Configuration.GetValue<TimeSpan>("Jwt:TokenLifetime")!;
        });        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapAccountEndpoints();

        await UserDbInitializer.EnsureUsers(app.Services, GetUserData());
        app.Run();
    }

    private static IEnumerable<(UserLoginData data, IEnumerable<string> roles)> GetUserData()
    {
        yield return (new UserLoginData
        {
            UserName = "alice",
            Email = "alice@bob.com",
            Password = "123Admin!"
        }, new List<string> { "Admin", "User" });

        yield return (new UserLoginData
        {
            UserName = "bob",
            Email = "bob@alice.com",
            Password = "123User!"
        }, new List<string> { "User" });
    }

}
