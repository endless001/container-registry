namespace ContainerRegistry.Core.Models;

public record Scope
{
    public string Type { get; init; }
    public string Namespace { get; init; }
    public string RepositoryName { get; init; }
    public string Action { get; init; }
}