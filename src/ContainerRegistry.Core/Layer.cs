namespace ContainerRegistry.Core;

public record struct Layer
{
    public long Size { get; init; }
    public Stream Content { get; init; }
}