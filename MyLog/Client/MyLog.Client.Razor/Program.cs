using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyLog.Client.Razor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie.HttpOnly = true; // Prevent JavaScript access to the cookie
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use cookies only on HTTPS
                options.Cookie.Name = "AuthCookie";
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // Set cookie expiration
                options.SlidingExpiration = true; // Renew the cookie automatically
            });
        builder.Services.AddAuthorization();
        builder.Services.AddRazorPages();

        builder.Services.AddHttpContextAccessor();

        builder.AddAuthenticatingHttpClient();
        builder.Services.AddSession();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseSession();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
