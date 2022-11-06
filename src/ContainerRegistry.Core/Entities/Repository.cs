namespace ContainerRegistry.Core.Entities;

public class Repository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long Downloads { get; set; }
    public RepositoryType Type { get; set; }
    public List<RepositoryTag> Tags { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}