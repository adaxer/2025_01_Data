using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyLog.Core.Contracts.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyLog.Client.Razor.Pages;

[Authorize]
public class MovementDetailsModel : PageModel
{
    private readonly HttpClient _client;

    public MovementDetailsModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _client = httpClientFactory.CreateClient("Api");
        var accessToken = httpContextAccessor.HttpContext.User.FindFirst("AccessToken")?.Value;

        if (!string.IsNullOrEmpty(accessToken))
        {
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    [BindProperty]
    public MovementDetailDto Detail { get; set; }

    [BindProperty]
    public int? DeliveryId { get; set; }
    [BindProperty]
    public int? PickUpId { get; set; }
    [BindProperty]
    public int? CargoPayerId { get; set; }
    [BindProperty]
    public string CargoNr { get; set; } = string.Empty;
    [BindProperty]
    public string UserName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Detail = (await _client.GetFromJsonAsync<MovementDetailDto>($"movement/{id}"))!;
        DeliveryId = Detail.DeliveryId;
        PickUpId = Detail.PickUpId;
        CargoPayerId = Detail.CargoPayerId;
        UserName = Detail.UserName;
        CargoNr = Detail.CargoNr;

        if (Detail == null)
        {
            return NotFound();
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var detail = new MovementDetailDto
        {
            Id = id,
            CargoNr = CargoNr,
            DeliveryId = DeliveryId,
            PickUpId = PickUpId,
            CargoPayerId = CargoPayerId,
            UserName = UserName
        };
        var response = await _client.PostAsJsonAsync($"movement", detail);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Movements");
        }
// todo        ErrorMessage=bla
        return Page();
    }
}
