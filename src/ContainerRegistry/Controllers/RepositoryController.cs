using System.Net;
using ContainerRegistry.Core.Models;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("api/v1/[controller]/{namespace}")]
[ApiController]
public class RepositoryController : ControllerBase
{
    private readonly IRepositoryService _repositoryService;
    private readonly IOrganizationService _organizationService;

    public RepositoryController(IRepositoryService repositoryService,
        IOrganizationService organizationService)
    {
        _repositoryService = repositoryService;
        _organizationService = organizationService;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(PagedList<RepositoryResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync(string @namespace, CancellationToken cancellationToken,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var result = await _repositoryService.GetAsync(@namespace, pageSize, pageIndex);
        return Ok(result);
    }

    [HttpGet("{name}")]
    [ProducesResponseType(typeof(RepositoryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAsync(string @namespace, string name, CancellationToken cancellationToken)
    {

        var result = await _repositoryService.GetAsync(@namespace, name);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpGet("{name}/tag")]
    public Task<IActionResult> GetTagAsync(string @namespace, string name)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{name}/tag/{version}")]
    public Task<IActionResult> GetTagAsync(string @namespace, string name, string version)
    {
        throw new NotImplementedException();
    }
}