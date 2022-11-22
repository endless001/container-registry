using ContainerRegistry.Core.Entities;
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

    public async Task<PaginatedItems<Organization>> GetAsync(string @namespace, int pageSize, int pageIndex)
    {
        var totalItems = await _context.Organizations
            .LongCountAsync();

        var itemsOnPage = await _context.Organizations
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        var model = new PaginatedItems<Organization>(pageIndex, pageSize, totalItems, itemsOnPage);
        return model;
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
}