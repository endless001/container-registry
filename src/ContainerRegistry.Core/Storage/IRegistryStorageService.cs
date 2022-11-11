namespace ContainerRegistry.Core.Storage;

public interface IRegistryStorageService
{
    Task<Layer> GetLayerAsync(string name, string digest, CancellationToken cancellationToken);
    Task<Manifest> GetManifestAsync(string name, string reference, CancellationToken cancellationToken);

    Task<Manifest> CreateManifestAsync(string name, string reference, Stream contents,
        CancellationToken cancellationToken);

    ValueTask<string> GetManifestHashAsync(string name, string reference,CancellationToken cancellationToken);

        Task CopyManifestAsync(string name, string reference, string hash, CancellationToken cancellationToken);
    Task UploadBlobAsync(string name, string uuid, Stream contents, CancellationToken cancellationToken);

    Task CopyBlobAsync(string name, string uuid, string digest, CancellationToken cancellationToken);
}