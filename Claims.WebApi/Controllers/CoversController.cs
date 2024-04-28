using Claims.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost("/ComputePremium")]
    public ActionResult ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_premiumComputeService.ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var covers = await _coversService.GetCoversAsync();

        return Ok(covers.Adapt<IEnumerable<Cover>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var cover = await _coversService.GetCoverAsync(id);

        return Ok(cover.Adapt<Cover>());
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        //cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        await _coversService.CreateCoverAsync(cover.Adapt<Domain.Cover>());
        await _auditerService.AuditCover(cover.Id, "POST");

        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _auditerService.AuditCover(id, "DELETE");
        await _coversService.DeleteCoverAsync(id);

        return NoContent();
    }
}