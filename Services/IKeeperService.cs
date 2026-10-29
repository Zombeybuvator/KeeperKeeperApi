using KeeperApi.Models;

namespace KeeperApi.Services;

public interface IKeeperService
{
    Task<Visitor?> RegisterVisitorAsync(string name);
    Task<List<Visitor>> GetAllVisitorsAsync();
    Task<Visitor?> GetVisitorByIdAsync(int id);
    Task<(bool Success, string Message, Visitor? Visitor)> PerformActionAsync(int visitorId, string actionType);
    Task<(bool Found, bool HasAccess)> CheckAccessAsync(int visitorId);
}
