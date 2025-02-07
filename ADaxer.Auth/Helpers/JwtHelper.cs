﻿using Microsoft.IdentityModel.Tokens;
using ADaxer.Auth.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ADaxer.Auth.Helpers;

public class JwtHelper
{
    public static string GenerateJwtToken(ApplicationUser user, IList<string> roles, ConfigureUserDbOptions options)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.EncryptionSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user!.UserName!),
            new Claim(ClaimTypes.Email, user!.Email!)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            options.TokenIssuer,
            options.TokenAudience,
            claims,
            expires: DateTime.UtcNow + options.TokenLifetime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static void ReadToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Extract claims
        var claims = jwtToken.Claims;
        foreach (var claim in claims)
        {
            Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
        }

        // Extract specific claims
        var userId = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var userName = claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

        Console.WriteLine($"User ID: {userId}");
        Console.WriteLine($"User Name: {userName}");
    }
}
