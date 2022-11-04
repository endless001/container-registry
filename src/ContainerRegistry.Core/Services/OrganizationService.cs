using ContainerRegistry.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Core.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IContext _context;

    public OrganizationService(IContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string @namespace, CancellationToken cancellationToken)
    {
        return await _context.Organizations.Where(o => o.Namespace == @namespace).AnyAsync(cancellationToken);
    }
}