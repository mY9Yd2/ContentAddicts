namespace ContentAddicts.Api.Models;

public class OtherName
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public Guid CreatorId { get; set; }
    public Creator Creator { get; set; } = null!;
}
