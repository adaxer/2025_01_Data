
using ADaxer.Auth;
using ADaxer.Auth.Endpoints;
using Microsoft.EntityFrameworkCore;
using MyLog.Core.Contracts;
using MyLog.Core.Contracts.Interfaces;
using MyLog.Core.Logic;
using MyLog.Core.Reporting;
using MyLog.Data.DataAccess;
using MyLog.Data.DataAccess.Repositories;
using MyLog.Services.Api.Extensions;
using Scalar.AspNetCore;

namespace MyLog.Services.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddConfiguredUserDb(o=>
        {
            o.UserDbConnectionString = builder.Configuration.GetConnectionString("UsersDb")!;
            o.EncryptionSecret = builder.Configuration.GetValue<string>("Jwt:Secret")!;
            o.TokenIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer")!;
            o.TokenAudience = builder.Configuration.GetValue<string>("Jwt:Audience")!;
            o.TokenLifetime = builder.Configuration.GetValue<TimeSpan>("Jwt:TokenLifetime")!;
        });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<MyLogContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyLogDb"));
        });

        // Eigene Services
        builder.Services.AddScoped<IMovementsService, MovementsService>();
        builder.Services.AddScoped<IMovementsRepository, MovementsRepository>();
        builder.Services.AddTransient<IReportService, ReportService>();

        var app = builder.Build();

        /* ------------------------------------------------- */
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapAccountEndpoints();
        app.MapEndpoints();
        // entspricht dem hier: WebApplicationExtensions.MapEndpoints(app);

        if (app.Environment.IsDevelopment())
        {
            var initializer = new MyLogInitializer(app.Services);
            await initializer.SeedTestDataAsync();
        }
        await app.RunAsync();
    }
}
