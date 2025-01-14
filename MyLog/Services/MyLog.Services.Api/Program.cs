
using Microsoft.EntityFrameworkCore;
using MyLog.Core.Logic;
using MyLog.Data.DataAccess;
using MyLog.Services.Api.Extensions;
using Scalar.AspNetCore;

namespace MyLog.Services.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<MyLogContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyLogDb"));
        });

        // Eigene Services
        builder.Services.AddScoped<MovementsService>();

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

        app.MapEndpoints();
        // entspricht dem hier: WebApplicationExtensions.MapEndpoints(app);

        app.Run();
    }
}
