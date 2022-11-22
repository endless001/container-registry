using ContainerRegistry.Core.Enums;

namespace ContainerRegistry.Core.Models;

public record Scope
{
    public string Type { get; init; }
    public string Namespace { get; init; }
    public string RepositoryName { get; init; }
    public ActionType Action { get; init; }
}