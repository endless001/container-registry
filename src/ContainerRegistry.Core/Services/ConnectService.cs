using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContainerRegistry.Core.Configuration;
using ContainerRegistry.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContainerRegistry.Core.Services;

public class ConnectService : IConnectService
{
    private readonly JwtBearerOptions _options;
    public ConnectService(IOptionsSnapshot<JwtBearerOptions> options)
    {
        _options = options.Value;
    }

    public async Task<Tokens> CreateTokenAsync(CancellationToken cancellationToken)
    {
        var issuer = _options.Issuer;
        var signKey = _options.SignKey;
        var claims = new[]
        {
            new Claim("email", "1319822160@qq.com")
        };

        var claimsIdentity = new ClaimsIdentity(claims);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return new Tokens
        {
            Token = token
        };
    }
}