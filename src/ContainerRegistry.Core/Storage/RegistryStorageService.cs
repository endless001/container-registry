namespace ContainerRegistry.Core.Storage;

public class RegistryStorageService : IRegistryStorageService
{
    private const string DockerManifestV2 = "application/vnd.docker.distribution.manifest.v2+json";
    private readonly IStorageService _storage;

    public RegistryStorageService(IStorageService storage)
    {
        _storage = storage;
    }

    private string ManifestPath(string name, string reference)
    {
        return Path.Combine(
            name,
            $"{reference}.json");
    }

    private string LayerPath(string name, string digest)
    {
        return Path.Combine(
            name,
            digest);
    }
}