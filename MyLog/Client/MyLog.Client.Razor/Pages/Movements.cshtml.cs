using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyLog.Core.Contracts.Models;

namespace MyLog.Client.Razor.Pages;

[Authorize]
public class MovementsModel : PageModel
{
    private HttpClient _client;
    private string? _accessToken;

    public MovementsModel(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
    {
        _client = factory.CreateClient("Api");
        _accessToken = _client.DefaultRequestHeaders.Authorization?.Parameter;
    }

    public List<MovementDto> Movements { get; private set; } = new();
    public string? AccessToken => _accessToken;

    public async Task OnGetAsync()
    {
        var movements = await _client.GetFromJsonAsync<MovementDto[]>("movements/20");
        Movements = movements!.ToList();
    }
}

