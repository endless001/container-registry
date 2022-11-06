using System.ComponentModel.DataAnnotations;

namespace ContainerRegistry.Core.Configuration;

public class DatabaseOptions
{
    public string Type { get; set; }

    [Required] public string ConnectionString { get; set; }
}