using System.Net;
using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class RepositoryController : ControllerBase
{
    private readonly IRepositoryService _repositoryService;

    public RepositoryController(IRepositoryService repositoryService)
    {
        _repositoryService = repositoryService;
    }

    [HttpGet("{organizationId:int}")]
    [ProducesResponseType(typeof(PaginatedItems<Repository>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync(int organizationId, [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var result = await _repositoryService.GetAsync(organizationId, pageSize, pageIndex);
        return Ok(result);
    }
}