using Claims.Domain.Exceptions;
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
        try
        {
            var cover = await _coversService.GetCoverAsync(id);

            return Ok(cover.Adapt<Cover>());
        }
        catch (CoverNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        if (cover.Id == null)
        {
            cover.Id = Guid.NewGuid().ToString();
        }
        
        await _coversService.CreateCoverAsync(cover.Adapt<Domain.Cover>());
        await _auditerService.AuditCover(cover.Id, "POST");

        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        try
        {
            await _coversService.DeleteCoverAsync(id);
            await _auditerService.AuditCover(id, "DELETE");

            return NoContent();
        }
        catch (CoverNotFoundException)
        {
            return NotFound();
        }

    }
}