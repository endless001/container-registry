namespace ContainerRegistry.Core.Entities;

public class RepositoryTag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RepositoryId { get; set; }
    public Repository Repository { get; set; }
}