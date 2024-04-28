using Claims.Domain.Contracts;
using Claims.Domain.Contracts.Exceptions;
using Claims.Services;
using Claims.WebApi.Contracts;
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
    public async Task<ActionResult<IEnumerable<GetCoverResponse>>> GetAsync()
    {
        var covers = await _coversService.GetCoversAsync();

        return Ok(covers.Adapt<IEnumerable<GetCoverResponse>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCoverResponse>> GetAsync(string id)
    {
        try
        {
            var cover = await _coversService.GetCoverAsync(id);

            return Ok(cover.Adapt<GetCoverResponse>());
        }
        catch (CoverNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateCoverRequest cover)
    {
        var coverId = await _coversService.CreateCoverAsync(cover.Adapt<Cover>());
        await _auditerService.AuditCover(coverId, "POST");

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