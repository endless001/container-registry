namespace ContainerRegistry.Core.Entities;

public class Organization
{
    public int Id { get; set; }
    public string Namespace { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Gravatar { get; set; }
    public string Email { get; set; }
    public List<User> Members { get; set; }
    public List<Repository> Repositories { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}