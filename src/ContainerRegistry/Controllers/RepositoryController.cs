using System.Net;
using ContainerRegistry.Core.Models;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("api/v1/[controller]/{namespace}")]
[ApiController]
[Authorize]
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
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAsync(string @namespace, CancellationToken cancellationToken,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var organization = await _organizationService.GetAsync(@namespace, cancellationToken);
        if (organization is null)
        {
            return NotFound();
        }

        var result = await _repositoryService.GetAsync(organization.Id, pageSize, pageIndex);
        return Ok(result);
    }

    [HttpGet("{repository}")]
    [ProducesResponseType(typeof(RepositoryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAsync(string @namespace, string repository, CancellationToken cancellationToken)
    {
        var organization = await _organizationService.GetAsync(@namespace, cancellationToken);
        if (organization is null)
        {
            return NotFound();
        }

        var result = await _repositoryService.GetAsync(organization.Id, repository);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpGet("{repository}/tags")]
    public Task<IActionResult> GetTagsAsync(string @namespace, string repository)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{repository}/tags/{version}")]
    public Task<IActionResult> GetTagsAsync(string @namespace, string repository, string version)
    {
        throw new NotImplementedException();
    }
}