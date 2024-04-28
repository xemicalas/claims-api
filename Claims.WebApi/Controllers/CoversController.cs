using Claims.Auditing;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly ICoversService _coversService;
    private readonly IAuditerService _auditerService;
    private readonly IPremiumComputeService _premiumComputeService;

    public CoversController(ILogger<CoversController> logger, ICoversService coversService, IAuditerService auditerService, IPremiumComputeService premiumComputeService)
    {
        _logger = logger;
        _coversService = coversService;
        _auditerService = auditerService;
        _premiumComputeService = premiumComputeService;
    }
    
    [HttpPost]
    public async Task<ActionResult> ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_premiumComputeService.ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var result = await _coversService.GetCoversAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var result = await _coversService.GetCoverAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        //cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        await _coversService.CreateCoverAsync(cover);
        await _auditerService.AuditCover(cover.Id, "POST");
        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(string id)
    {
        _auditerService.AuditCover(id, "DELETE");
        return _coversService.DeleteCoverAsync(id);
    }
}