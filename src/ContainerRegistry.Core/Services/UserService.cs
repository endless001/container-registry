using ContainerRegistry.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Core.Services;

public class UserService : IUserService
{
    private readonly IContext _context;

    public UserService(IContext context)
    {
        _context = context;
    }

    public Task<User> FindAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SynchronizeUserAsync(string accessToken)
    {
        var identity = new
        {
            UserName = "lq",
            Email = "zze@live.com",
            Avatar = "https://avatars.githubusercontent.com/u/19401853?v=4"
        };
        var user = await _context.Users.Where(u => u.UserName == identity.UserName).FirstOrDefaultAsync();
        if (user is null)
        {
            user = new User
            {
                Token = accessToken,
                Secret = "lq",
                UserName = identity.UserName,
                Email = identity.Email,
                Expiry = "lq",
                Refresh = TimeSpan.FromMinutes(1),
                Avatar = identity.Avatar,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };
            _context.Users.AddAsync(user);
        }
        else
        {
            user.Token = accessToken;
            user.UserName = identity.UserName;
            user.Email = identity.Email;
            user.Avatar = identity.Avatar;
            user.Updated = DateTime.UtcNow;

            _context.Users.Update(user);
        }

        await _context.SaveChangesAsync(new CancellationToken());
    }
}