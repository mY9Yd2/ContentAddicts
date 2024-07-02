namespace ContentAddicts.Api.Models;

public class Creator
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<OtherName> OtherNames { get; } = [];
}
