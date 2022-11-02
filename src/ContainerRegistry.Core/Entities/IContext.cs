using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ContainerRegistry.Core.Entities;

public interface IContext
{
    DatabaseFacade Database { get; }
    
}