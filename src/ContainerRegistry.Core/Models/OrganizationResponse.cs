namespace ContainerRegistry.Core.Models;

public class OrganizationResponse
{
    public int Id { get; set; }
    public string Namespace { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Gravatar { get; set; }
    public string Email { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}