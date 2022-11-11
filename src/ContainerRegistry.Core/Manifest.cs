namespace ContainerRegistry.Core;

public readonly record struct Manifest
{
    public string MediaType { get; init; }
    public string Digest { get; init; }

    public long Size { get; init; }
    public string Hash { get; init; }
    public Stream Content { get; init; }
}