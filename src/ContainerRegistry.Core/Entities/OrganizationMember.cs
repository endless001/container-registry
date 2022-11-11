namespace ContainerRegistry.Core.Entities;

public class OrganizationMember
{
    public int OrganizationId { get; set; }
    public int MemberId { get; set; }
    public Organization Organization { get; set; }
    public User User { get; set; }
}