namespace MyLog.Client.Razor;

public class AccessTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccessTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Retrieve the access token from the session
        var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

        if (!string.IsNullOrEmpty(accessToken))
        {
            // Add the access token to the Authorization header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        // Proceed with the request
        return await base.SendAsync(request, cancellationToken);
    }
}
