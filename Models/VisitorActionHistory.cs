namespace KeeperApi.Models;

public class VisitorActionHistory
{
    public int Id { get; set; }
    public int VisitorId { get; set; } 
    public string ActionType { get; set; } = string.Empty;
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
