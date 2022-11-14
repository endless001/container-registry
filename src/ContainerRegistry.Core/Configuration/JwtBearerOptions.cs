using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ContainerRegistry.Core.Configuration;

public class JwtBearerOptions
{
    public string Issuer { get; set; }
    public string SignKey { get; set; }
}