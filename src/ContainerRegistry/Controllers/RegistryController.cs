using ContainerRegistry.Core.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("v2")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RegistryController : ControllerBase
{
    private readonly IRegistryStorageService _registryStorage;
    public RegistryController(IRegistryStorageService registryStorage)
    {
        _registryStorage = registryStorage;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpHead("{name}/blobs/{rawDigest}")]
    public Task<IActionResult> Head(string name, string rawDigest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{name}/blobs/{digest}")]
    public async Task<IActionResult> Get(string name, string digest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{name}/blobs/uploads")]
    public async Task<IActionResult> Post(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [DisableRequestSizeLimit]
    [HttpPatch("{name}/blobs/uploads/{uuid}")]
    public async Task<IActionResult> Patch(string name, string uuid, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{name}/blobs/uploads/{uuid}")]
    public async Task<IActionResult> Put(string name, string uuid, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpHead("{name}/manifests/{reference}")]
    public async Task<IActionResult> HeadManifest(string name, string reference, CancellationToken cancellationToken)
    {
 
        throw new NotImplementedException();
    }

    [HttpGet("{name}/manifests/{reference}")]
    public async Task<IActionResult> GetManifest(string name, string reference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{name}/manifests/{reference}")]
    public async Task<IActionResult> PutManifest(string name, string reference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}