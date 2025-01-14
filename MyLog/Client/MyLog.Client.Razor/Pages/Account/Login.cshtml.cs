//var response = await _client.PostAsJsonAsync("auth/login", new UserLoginData(Username, Password));
//Token token = (await response.Content.ReadFromJsonAsync<Token>())!;
//_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken); 
//var userInfo = await _client.GetFromJsonAsync<UserInfo>("auth/userinfo");

//var claims = new List<Claim>
//{
//    new Claim(ClaimTypes.Name, userInfo!.UserName!),
//    new Claim(ClaimTypes.Email, userInfo!.Email!),
//}
//.Concat(userInfo.Roles.Select(r=> new Claim(ClaimTypes.Role, r)))
//.ToList();
//var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
//HttpContext.Session.SetString("AccessToken", token.AccessToken);

//return Redirect(returnUrl);

using System.Security.Claims;
using System.Text.Json.Serialization;
using ADaxer.Auth.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyLog.Client.Razor.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = default!;

    [BindProperty]
    public string Password { get; set; } = default!;

    [BindProperty(SupportsGet = true)] // Allow this to bind from query string during GET
    public string ReturnUrl { get; set; } = "/Index";
    public string ErrorMessage { get; set; } = default!;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Username and Password are required.";
            return Page();
        }

        // Call the API to authenticate the user
        var tokenResponse = await AuthenticateWithApiAsync(Username, Password);

        if (tokenResponse == null)
        {
            ErrorMessage = "Invalid login attempt.";
            return Page();
        }

        // Create user claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, tokenResponse.UserName),
            new Claim("AccessToken", tokenResponse.AccessToken),
            new Claim("RefreshToken", tokenResponse.RefreshToken)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Sign in the user
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = true, // Keeps the session alive even after the browser is closed
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Set token expiration
            });

        return RedirectToPage(ReturnUrl);
    }

    private async Task<TokenResponse> AuthenticateWithApiAsync(string username, string password)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7065"); // Base URL of your API

        var content = new UserLoginData() { UserName = Username, Password = Password };

        var response = await httpClient.PostAsJsonAsync("/auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

            return tokenResponse;
        }

        return null;
    }

    public class TokenResponse
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
