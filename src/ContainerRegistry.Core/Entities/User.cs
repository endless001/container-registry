namespace ContainerRegistry.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Secret { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Token { get; set; }
    public TimeSpan Refresh { get; set; }
    public string Expiry { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}