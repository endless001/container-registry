using System.Text.Json;
using ContainerRegistry.Protocol.Models;

namespace ContainerRegistry.Protocol.GitHub;

public class GitHubClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GitHubClient(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    public async Task<GitHubUserResponse> GetUserAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("GitHub");
        var response = await httpClient.GetAsync("/user");

        response.EnsureSuccessStatusCode();
        var userResponse = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<GitHubUserResponse>(userResponse);
    }
}