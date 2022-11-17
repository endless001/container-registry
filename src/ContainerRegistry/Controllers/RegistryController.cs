using ContainerRegistry.Core.Storage;
using ContainerRegistry.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("v2")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RegistryController : ControllerBase
{
    private readonly IRegistryStorageService _registryStorage;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpHead("{name}/blobs/{rawDigest}")]
    public async Task<IActionResult> Head(string name, string rawDigest)
    {
        var digest = rawDigest.Split(":").Last();
        try
        {
            var layer = await _registryStorage.GetLayerAsync(name, digest, new CancellationToken());
            Response.Headers.Add("content-length", layer.Size.ToString());
            Response.Headers.Add("docker-content-digest", rawDigest);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpGet("{name}/blobs/{digest}")]
    public async Task<IActionResult> Get(string name, string digest)
    {
        var hash = digest.Split(":").Last();
        try
        {
            var layer = await _registryStorage.GetLayerAsync(name, hash, new CancellationToken());
            await layer.Content.CopyToAsync(Response.Body);
            Response.Headers.Add("content-length", layer.Size.ToString());
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpPost("{name}/blobs/uploads")]
    public IActionResult Post(string name)
    {
        var uuid = Guid.NewGuid().ToString();
        Response.Headers.Add("location", "/v2/" + name + "/blobs/uploads/" + uuid);
        Response.Headers.Add("range", "0-0");
        Response.Headers.Add("content-length", "0");
        Response.Headers.Add("docker-upload-uuid", uuid);
        return Accepted();
    }

    [DisableRequestSizeLimit]
    [HttpPatch("{name}/blobs/uploads/{uuid}")]
    public async Task<IActionResult> Patch(string name, string uuid, CancellationToken cancellationToken)
    {
        await using (var ms = await Request.GetUploadStreamOrNullAsync(cancellationToken))
        {
            await _registryStorage.UploadBlobAsync(name, uuid, ms, cancellationToken);
            Response.Headers["range"] = $"0-{ms.Position - 1}";
        }

        Response.Headers["docker-upload-uuid"] = uuid;
        Response.Headers["location"] = $"/v2/{name}/blobs/uploads/{uuid}";
        Response.Headers["content-length"] = "0";
        Response.Headers["docker-distribution-api-version"] = "registry/2.0";
        return Accepted();
    }

    [HttpPut("{name}/blobs/uploads/{uuid}")]
    public async Task<IActionResult> Put(string name, string uuid, CancellationToken cancellationToken)
    {
        var rawDigest = Request.Query["digest"].ToString();
        var digest = rawDigest.Split(":").Last();
        Response.Headers.Add("content-length", "0");
        Response.Headers.Add("docker-content-digest", rawDigest);
        await _registryStorage.CopyBlobAsync(name, uuid, digest, cancellationToken);
        return Created($"/v2/{name}/blobs/{digest}", string.Empty);
    }

    [HttpHead("{name}/manifests/{reference}")]
    public async Task<IActionResult> HeadManifest(string name, string reference, CancellationToken cancellationToken)
    {
        try
        {
            var hash = await _registryStorage.GetManifestHashAsync(name, reference, cancellationToken);
            var manifest = await _registryStorage.GetManifestAsync(name, reference, cancellationToken);
            Response.Headers.Add("docker-content-digest", $"sha256:{hash}");
            Response.Headers.Add("content-type", manifest.MediaType);
            Response.Headers.Add("content-length", manifest.Size.ToString());
            return Ok();
        }
        catch
        {
            return NoContent();
        }
    }

    [HttpGet("{name}/manifests/{reference}")]
    public async Task<IActionResult> GetManifest(string name, string reference)
    {
        var hash = reference.Split(":").Last();
        try
        {
            var manifest = await _registryStorage.GetManifestAsync(name, hash, new CancellationToken());

            Response.Headers.Add("docker-content-digest", $"sha256:{manifest.Hash}");
            Response.Headers.Add("content-type", manifest.MediaType);
            Response.Headers.Add("content-length", manifest.Size.ToString());
            await manifest.Content.CopyToAsync(Response.Body);

            return Ok();
        }
        catch
        {
            return NoContent();
        }
    }

    [HttpPut("{name}/manifests/{reference}")]
    public async Task<IActionResult> PutManifest(string name, string reference, CancellationToken cancellationToken)
    {

        await using (var ms = await Request.GetUploadStreamOrNullAsync(cancellationToken))
        {
            await _registryStorage.CreateManifestAsync(name, reference, ms, cancellationToken);
        }

        var hash = await _registryStorage.GetManifestHashAsync(name, reference, cancellationToken);
        Response.Headers.Add("docker-content-digest", $"sha256:{hash}");
        await _registryStorage.CopyManifestAsync(name, reference, hash, cancellationToken);
        return Created($"/v2/{name}/manifests/{reference}", null);
    }
}