using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public List<MovementDto> Movements { get; private set; }

    public async Task OnGetAsync()
    {
        var movements = await _client.GetFromJsonAsync<MovementDto[]>("movements/20");
        Movements = movements!.ToList();
    }

    [HttpPost("/ delete /{id}")]
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var response = await _client.DeleteAsync($"/movements/{id}");
        if (response.IsSuccessStatusCode)
        {
            // Reload the list of movements after deletion
            Movements = await _client.GetFromJsonAsync<List<MovementDto>>("/movements/20");
            return Page();
        }
        else
        {
            // Handle error
            ModelState.AddModelError(string.Empty, "Failed to delete the item.");
            return Page();
        }
    }
}

