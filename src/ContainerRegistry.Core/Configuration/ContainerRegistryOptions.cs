namespace ContainerRegistry.Core.Configuration;

public class ContainerRegistryOptions
{
    public DatabaseOptions Database { get; set; }
    public StorageOptions Storage { get; set; }
}