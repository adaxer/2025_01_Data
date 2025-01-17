using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ADaxer.Auth.User;
using System.Security.Claims;
using System.Text;

namespace ADaxer.Auth;

public static class BuilderExtensions
{
    public static IServiceCollection AddConfiguredUserDb(this IServiceCollection services, Action<ConfigureUserDbOptions> configure = default!)
    {
        var options = new ConfigureUserDbOptions();
        if (configure != default)
        {
            configure(options);
        }

        services.AddSingleton(options);

        services.AddDbContext<UserDbContext>(o =>
            o.UseSqlServer(options.UserDbConnectionString));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>();

        var key = Encoding.ASCII.GetBytes(options.EncryptionSecret);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            options.AddPolicy("UserOnly", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
            options.AddPolicy("AdminOrUser", policy => policy.RequireAssertion(context =>
                   context.User.HasClaim(c => (c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "User")))));
            options.AddPolicy("BasicAuth", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
