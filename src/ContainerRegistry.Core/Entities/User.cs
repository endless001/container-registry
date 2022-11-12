namespace ContainerRegistry.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Secret { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Token { get; set; }
    public string Refresh { get; set; }
    public TimeSpan? Expiry { get; set; }
    public List<OrganizationMember> OrganizationMembers { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}