using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyLog.Core.Contracts.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyLog.Client.Razor.Pages
{
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Detail = await _client.GetFromJsonAsync<MovementDetailDto>($"movement/{id}");
            if (Detail == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var response = await _client.PostAsJsonAsync($"movement", Detail);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Movements");
            }
            return Page();
        }
    }
}
