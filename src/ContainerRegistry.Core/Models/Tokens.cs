namespace ContainerRegistry.Core.Models;

public class Tokens
{
    public string Token { get; set; }
    public DateTime Issued { get; set; }
    public int Expires { get; set; }
}