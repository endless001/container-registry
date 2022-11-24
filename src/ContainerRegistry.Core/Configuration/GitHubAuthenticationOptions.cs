namespace ContainerRegistry.Core.Configuration;

public class GitHubAuthenticationOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string CallbackPath { get; set; }
}