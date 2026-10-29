using KeeperApi.Data;
using KeeperApi.Models;
using Microsoft.EntityFrameworkCore; 

namespace KeeperApi.Services;

public class KeeperService : IKeeperService
{
    private readonly AppDbContext _context;

    public KeeperService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Visitor?> RegisterVisitorAsync(string name)
    {
        var visitor = new Visitor { Name = name, TrustLevel = 0 };
        _context.Visitors.Add(visitor);
        await _context.SaveChangesAsync();
        return visitor;
    }

    public async Task<List<Visitor>> GetAllVisitorsAsync() => 
        await _context.Visitors.Include(v => v.History).ToListAsync();

    public async Task<Visitor?> GetVisitorByIdAsync(int id) => 
        await _context.Visitors.Include(v => v.History).FirstOrDefaultAsync(v => v.Id == id);

    public async Task<(bool Success, string Message, Visitor? Visitor)> PerformActionAsync(int visitorId, string actionType)
    {
        var visitor = await _context.Visitors.Include(v => v.History).FirstOrDefaultAsync(v => v.Id == visitorId);
        if (visitor == null) return (false, "Посетитель не найден", null);

        int trustModifier = actionType switch
        {
            "Introduce" => 20,
            "OfferHelp" => 30,
            "Threaten" => -20,
            _ => -1
        };

        if (trustModifier == -1) return (false, "Неизвестное действие, Допустимы: Introduce, OfferHelp, Threaten", null);


        if (visitor.History.Any(h => h.ActionType == actionType))
        {
            return (false, "Действие уже было выполнено данным посетителем", null);
        }

        visitor.TrustLevel = Math.Clamp(visitor.TrustLevel + trustModifier, 0, 100);
        visitor.History.Add(new VisitorActionHistory { ActionType = actionType });
        
        await _context.SaveChangesAsync();
        return (true, "Действие выполнено", visitor);
    }

    public async Task<(bool Found, bool HasAccess)> CheckAccessAsync(int visitorId)
    {
        var visitor = await _context.Visitors.FindAsync(visitorId);
        if (visitor == null) return (false, false);
        
        return (true, visitor.TrustLevel >= 50);
    }
}
