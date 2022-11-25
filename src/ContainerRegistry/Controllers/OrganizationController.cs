using System.Net;
using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Mappers;
using ContainerRegistry.Core.Models;
using ContainerRegistry.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContainerRegistry.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<OrganizationResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var result = await _organizationService.GetAsync(pageSize, pageIndex);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] OrganizationRequest request,
        CancellationToken cancellationToken)
    {
        var organization = request.ToOrganizationModel<Organization>();
        var result = await _organizationService.CreateAsync(organization, cancellationToken);
        return Created(string.Empty, null);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] OrganizationRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{namespace}/member")]
    [ProducesResponseType(typeof(IEnumerable<MemberResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetMemberAsync(string @namespace, CancellationToken cancellationToken)
    {
        var members = await _organizationService.GetMemberAsync(@namespace, cancellationToken);
        return Ok(members);
    }

    [HttpPost("{namespace}/member")]
    public async Task<IActionResult> AddMemberAsync(string @namespace,CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}