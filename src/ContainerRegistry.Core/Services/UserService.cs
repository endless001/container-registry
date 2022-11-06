using ContainerRegistry.Core.Entities;

namespace ContainerRegistry.Core.Services;

public class UserService : IUserService
{
    
    public Task<User> FindAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> SynchronizeUserAsync()
    {
        throw new NotImplementedException();
    }
}