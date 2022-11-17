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

    public ValueTask<User> FindAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Users.FindAsync(id);
    }

    public async ValueTask<bool> ValidateAsync(string userName, string secret)
    {
        return await _context.Users.Where(u => u.UserName == userName && u.Secret == secret).AnyAsync();
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
                UserName = identity.UserName,
                Email = identity.Email,
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