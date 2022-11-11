using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using ContainerRegistry.Core.Extensions;

namespace ContainerRegistry.Core.Storage;

public class RegistryStorageService : IRegistryStorageService
{
    private const string DockerManifestV2 = "application/vnd.docker.distribution.manifest.v2+json";
    private readonly IStorageService _storage;

    public RegistryStorageService(IStorageService storage)
    {
        _storage = storage;
    }

    public async Task<Layer> GetLayerAsync(string name, string digest, CancellationToken cancellationToken)
    {
        var contents = await GetStreamAsync(name, digest, LayerPath, cancellationToken);

        var result = new Layer
        {
            Size = contents.Length,
            Content = contents
        };
        return result;
    }

    public async Task<Manifest> GetManifestAsync(string name, string reference,
        CancellationToken cancellationToken)

    {
    
        var text = await GetStringAsync(name, reference, ManifestPath, cancellationToken);
        var manifest = JsonNode.Parse(text);

        if (manifest is null) throw new NotImplementedException("Got a manifest but it was null");

        if ((string?)manifest["mediaType"] != DockerManifestV2)
        {
            throw new NotImplementedException($"Do not understand the mediaType {manifest["mediaType"]}");
        }

        var mediaType = (string?)manifest["mediaType"];
        var config = manifest["config"];
        var digest = (string?)config["digest"];
        var contents = await GetStreamAsync(name, reference, ManifestPath, cancellationToken);

        var result = new Manifest()
        {
            MediaType = mediaType,
            Size = contents.Length,
            Digest = digest,
            Content = contents
        };
        return result;
    }

    public async Task<Manifest> CreateManifestAsync(string name, string reference, Stream contents,
        CancellationToken cancellationToken)
    {

        var manifestPath = ManifestPath(name, reference);
        var putResult = await _storage.PutAsync(manifestPath, contents, string.Empty, cancellationToken);
        if (putResult == StoragePutResult.Conflict)
        {
            throw new InvalidOperationException();
        }

        var fileSize = contents.Length;

        var result = new Manifest
        {
            MediaType = DockerManifestV2,
            Size = fileSize
        };
        return result;
    }

    public async ValueTask<string> GetManifestHashAsync(string name, string reference, CancellationToken cancellationToken)
    {
        var contents = await GetStreamAsync(name, reference, ManifestPath, cancellationToken);
        return await contents.AsHashAsync();
    }

    public async Task CopyManifestAsync(string name, string reference, string hash, CancellationToken cancellationToken)
    {
        var sourcePath = ManifestPath(name, reference);
        var targetPath = ManifestPath(name, hash);
        await _storage.CopyAsync(sourcePath, targetPath, cancellationToken);
    }

    public async Task UploadBlobAsync(string name, string uuid, Stream contents, CancellationToken cancellationToken)
    {
        var path = LayerPath(name, uuid);

        var result = await _storage.PutAsync(path, contents, string.Empty, cancellationToken);
        if (result == StoragePutResult.Conflict)
        {
            throw new InvalidOperationException();
        }
    }

    public async Task CopyBlobAsync(string name, string uuid, string digest, CancellationToken cancellationToken)
    {
        var sourcePath = LayerPath(name, uuid);
        var targetPath = LayerPath(name, digest);
        await _storage.CopyAsync(sourcePath, targetPath, cancellationToken);
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

    private async Task<string> GetStringAsync(
        string name,
        string reference,
        Func<string, string, string> pathFunc,
        CancellationToken cancellationToken)
    {
        var path = pathFunc(name, reference);

        try
        {
            var stream = await _storage.GetAsync(path, cancellationToken);

            using var reader = new StreamReader(
                stream,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 1024,
                leaveOpen: true);
            return await reader.ReadToEndAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw ex;
        }
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