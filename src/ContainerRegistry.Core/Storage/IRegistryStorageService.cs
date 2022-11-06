namespace ContainerRegistry.Core.Storage;

public interface IRegistryStorageService
{
    Task<Descriptor> GetImageManifestAsync(string name, string reference, CancellationToken cancellationToken);

    Task<Descriptor> SaveImageManifestAsync(string name, string reference, Stream contents,
        CancellationToken cancellationToken);

    Task UploadBlobAsync(string name, string digest, Stream contents, CancellationToken cancellationToken);
}