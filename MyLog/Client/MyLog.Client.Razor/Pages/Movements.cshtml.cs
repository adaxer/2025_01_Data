using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyLog.Core.Contracts.Models;

namespace MyLog.Client.Razor.Pages;

[Authorize]
public class MovementsModel : PageModel
{
    private HttpClient _client;

    public MovementsModel(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
    {
        _client = factory.CreateClient("Api");
        var accessToken = httpContextAccessor.HttpContext.User.FindFirst("AccessToken")?.Value;

        if (!string.IsNullOrEmpty(accessToken))
        {
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    public async Task OnGetAsync()
    {
        var movements = await _client.GetFromJsonAsync<MovementDto[]>("movements/2");
    }
}
