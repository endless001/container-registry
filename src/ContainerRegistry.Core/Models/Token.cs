namespace ContainerRegistry.Core.Models;

public class Token
{
    public string AccessToken { get; set; }
    public DateTime Issued { get; set; }
    public int Expires { get; set; }
}