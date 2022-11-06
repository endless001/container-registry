namespace ContainerRegistry.Core;

public readonly record struct Descriptor
{
    public string MediaType { get; init; }

    public string Digest { get; init; }

    public string? UncompressedDigest { get; init; }

    public long Size { get; init; }

    public Stream Content { get; init; }
}