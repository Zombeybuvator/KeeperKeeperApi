namespace KeeperApi.Models;

public class Visitor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TrustLevel { get; set; } = 0;
    public List<VisitorActionHistory> History { get; set; } = new();
}
