using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using Aliyun.OSS;
using ContainerRegistry.Core.Storage;
using Microsoft.Extensions.Options;

namespace ContainerRegistry.Aliyun;

public class AliyunStorageService : IStorageService
{
    private const string Separator = "/";
    private readonly string _bucket;
    private readonly string _prefix;
    private readonly OssClient _client;

    public AliyunStorageService(IOptionsSnapshot<AliyunStorageOptions> options, OssClient client)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        _bucket = options.Value.Bucket;
        _prefix = options.Value.Prefix;
        _client = client ?? throw new ArgumentNullException(nameof(client));

        if (!string.IsNullOrEmpty(_prefix) && !_prefix.EndsWith(Separator))
            _prefix += Separator;
    }

    public async Task<Stream> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        var ossObject = await Task.Factory.FromAsync(_client.BeginGetObject, _client.EndGetObject, _bucket,
            PrepareKey(path), null);

        return ossObject.HttpStatusCode switch
        {
            HttpStatusCode.OK => ossObject.Content,
            _ => throw new NoNullAllowedException()
        };
    }

    public Task<Uri> GetDownloadUriAsync(string path, CancellationToken cancellationToken = default)
    {
        var uri = _client.GeneratePresignedUri(_bucket, PrepareKey(path));
        return Task.FromResult(uri);
    }

    public async Task<StoragePutResult> PutAsync(string path, Stream content, string contentType,
        CancellationToken cancellationToken = default)
    {
        var metadata = new ObjectMetadata
        {
            ContentType = contentType,
        };

        var putResult = await Task<PutObjectResult>.Factory.FromAsync(_client.BeginPutObject, _client.EndPutObject,
            _bucket, PrepareKey(path), content, metadata);
        return putResult.HttpStatusCode switch
        {
            HttpStatusCode.OK => StoragePutResult.Success,
            _ => StoragePutResult.Success
        };
    }
    public async Task CopyAsync(string source, string target, CancellationToken cancellationToken = default)
    {
        var request = new CopyObjectRequest(_bucket, PrepareKey(source), _bucket, PrepareKey(target));

        var result = _client.CopyObject(request);

    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        _client.DeleteObject(_bucket, PrepareKey(path));
        return Task.CompletedTask;
    }

    private string PrepareKey(string path)
    {
        return _prefix + path.Replace("\\", Separator);
    }
}