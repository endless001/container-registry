using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Extensions;
using ContainerRegistry.Core.Mappers;
using ContainerRegistry.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Core.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IContext _context;

    public OrganizationService(IContext context)
    {
        _context = context;
    }

    public async ValueTask<bool> ExistsAsync(string @namespace, CancellationToken cancellationToken)
    {
        return await _context.Organizations.Where(o => o.Namespace == @namespace).AnyAsync(cancellationToken);
    }

    public async Task<OrganizationResponse> GetAsync(string @namespace, CancellationToken cancellationToken)
    {
        return null;
    }

    public async Task<PagedList<OrganizationResponse>> GetAsync(int pageSize, int pageIndex)
    {
        var organizations = await _context.Organizations
            .Include(x=>x.OrganizationMembers)
            .PageBy(x => x.Id, pageIndex, pageSize).ToListAsync();
        var totalCount = await _context.Organizations.LongCountAsync();

        var result = new PagedList<OrganizationResponse>(pageIndex, pageSize, totalCount,
            organizations.ToOrganizationModel<List<OrganizationResponse>>());
        return result;
    }

    public async ValueTask<bool> CreateAsync(Organization organization, CancellationToken cancellationToken)
    {
        await _context.Organizations.AddAsync(organization, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public ValueTask<bool> UpdateAsync(Organization organization, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MemberResponse>> GetMemberAsync(string @namespace, CancellationToken cancellationToken)
    {
        var members = await _context.OrganizationMembers
            .Include(x => x.User)
            .Include(x => x.Organization)
            .Where(x => x.Organization.Namespace == @namespace)
            .ToListAsync(cancellationToken);

        var result = members.Select(x => new MemberResponse
        {
            MemberId = x.MemberId,
            MemberName = x.User.UserName
        }).ToList();
        return result;
    }
}