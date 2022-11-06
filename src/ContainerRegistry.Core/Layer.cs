namespace ContainerRegistry.Core;

public record struct Layer
{
    public Descriptor Descriptor { get; private set; }

    public Stream Content { get; private init; }
}