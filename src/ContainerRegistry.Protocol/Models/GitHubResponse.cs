using System.Text.Json.Serialization;

namespace ContainerRegistry.Protocol.Models;

public class GitHubUserResponse
{
    [JsonPropertyName("login")]
    public string Login { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("avatar_url")]
    public string Avatar { get; set; }
}