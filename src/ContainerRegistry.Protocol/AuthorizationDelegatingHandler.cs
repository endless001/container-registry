using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ContainerRegistry.Protocol;

public class AuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {

        var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorization))
        {
            request.Headers.Add("Authorization", new List<string> { authorization });
        }

        var token = await GetToken();

        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    async Task<string> GetToken()
    {
        const string ACCESS_TOKEN = "access_token";

        return await _httpContextAccessor.HttpContext
            .GetTokenAsync(ACCESS_TOKEN);
    }
}