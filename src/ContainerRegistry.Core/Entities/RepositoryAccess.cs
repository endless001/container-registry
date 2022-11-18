namespace ContainerRegistry.Core.Entities;

public class RepositoryAccess
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int RepositoryId { get; set; }
    public int Action { get; set; }
    public Repository Repository { get; set; }
}