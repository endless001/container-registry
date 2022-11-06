using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace ContainerRegistry.Core.Storage;


public class RegistryStorageService : IRegistryStorageService
{
    private const string DockerManifestV2 = "application/vnd.docker.distribution.manifest.v2+json";
    private const string RegistryPathPrefix = "registry";
    private readonly IStorageService _storage;

    public RegistryStorageService(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task<Descriptor> GetImageManifestAsync(string name, string reference,CancellationToken cancellationToken)
    {
        var contents = await GetStreamAsync(name, reference, ManifestPath, cancellationToken);
        var s = string.Empty;

        var manifest = JsonNode.Parse(s);
        
        if (manifest is null) throw new NotImplementedException("Got a manifest but it was null");
        
        if ((string?)manifest["mediaType"] != DockerManifestV2)
        {
            throw new NotImplementedException($"Do not understand the mediaType {manifest["mediaType"]}");
        }

        var mediaType = (string?)manifest["mediaType"];
        var config = manifest["config"];
        var digest = (string?)config["digest"];

        Descriptor descriptor = new()
        {
            MediaType = mediaType,
            Size = contents.Length,
            Digest = digest,
            Content = contents
        };
        return descriptor;
        
    }

    public async Task<Descriptor> SaveImageManifestAsync(string name, string reference, Stream contents,
        CancellationToken cancellationToken)
    {
        var hash = new byte[SHA256.HashSizeInBytes];
        var manifestPath = ManifestPath(name, reference);
        var result = await _storage.PutAsync(manifestPath, contents, string.Empty, cancellationToken);
        if (result == StoragePutResult.Conflict)
        {
            throw new InvalidOperationException();
        }

        SHA256.HashData(contents, hash);
        var fileSize = contents.Length;
        var contentHash = Convert.ToHexString(hash).ToLowerInvariant();
        var uncompressedContentHash = string.Empty;
        Descriptor descriptor = new()
        {
            MediaType = DockerManifestV2,
            Size = fileSize,
            Digest = $"sha256:{contentHash}",
            UncompressedDigest = $"sha256:{uncompressedContentHash}",
        };
        return descriptor;
    }

    public Task UploadBlobAsync(string name, string digest, Stream contents, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<Stream> GetStreamAsync(
        string name,
        string reference,
        Func<string, string, string> pathFunc,
        CancellationToken cancellationToken)
    {
        var path = pathFunc(name, reference);

        try
        {
            return await _storage.GetAsync(path, cancellationToken);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private string ManifestPath(string name,string reference)
    {
        return Path.Combine(
            RegistryPathPrefix,
            name,
            reference,
            $"{name}.{reference}.json");
    }
}