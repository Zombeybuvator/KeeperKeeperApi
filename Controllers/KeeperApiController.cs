using KeeperApi.Models;
using KeeperApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KeeperApi.Controllers;

[ApiController]
[Route("api/visitors")] 
public class VisitorsController : ControllerBase
{
    private readonly IKeeperService _keeperService;

    public VisitorsController(IKeeperService keeperService)
    {
        _keeperService = keeperService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Имя не может быть пустым");
        var visitor = await _keeperService.RegisterVisitorAsync(name);
        return CreatedAtAction(nameof(GetById), new { id = visitor!.Id }, visitor);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _keeperService.GetAllVisitorsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var visitor = await _keeperService.GetVisitorByIdAsync(id);
        if (visitor == null) return NotFound("Посетитель не найден");
        return Ok(visitor);
    }

    [HttpPost("{id}/actions")]
    public async Task<IActionResult> PerformAction(int id, [FromBody] ActionRequest request)
    {
        var result = await _keeperService.PerformActionAsync(id, request.Action);
        if (!result.Success)
        {
            if (result.Message == "Посетитель не найден") return NotFound(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(new { 
            message = result.Message, 
            currentTrustLevel = result.Visitor!.TrustLevel, 
            action = request.Action 
        });
    }

    [HttpGet("{id}/access")]
    public async Task<IActionResult> CheckAccess(int id)
    {
        var (found, hasAccess) = await _keeperService.CheckAccessAsync(id);
        if (!found) return NotFound("Посетитель не найден");
        return Ok(new { allowed = hasAccess });
    }
}
