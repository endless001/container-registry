namespace ContainerRegistry.Core.Entities;

public class Repository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long Downloads { get; set; }
    public int OrganizationId { get; set; }
    public int RepositoryTypeId { get; set; }
    public Organization Organization { get; set; }
    public RepositoryType Type { get; set; }
    public List<RepositoryTag> Tags { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}