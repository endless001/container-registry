namespace ContainerRegistry.Core.Entities;

public class Repository
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<RepositoryTag> Tags { get; set; }
}