using Claims.Domain.Contracts;
using Claims.Domain.Contracts.Exceptions;
using Claims.Services;
using Claims.WebApi.Contracts;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly ICoversService _coversService;
    private readonly IPremiumComputeService _premiumComputeService;
    private readonly IValidator<CreateCoverRequest> _validator;

    public CoversController(
        ILogger<CoversController> logger,
        ICoversService coversService,
        IPremiumComputeService premiumComputeService,
        IValidator<CreateCoverRequest> validator
        )
    {
        _logger = logger;
        _coversService = coversService;
        _premiumComputeService = premiumComputeService;
        _validator = validator;
    }

    /// <summary>
    /// Computes premium by date intervals and coverType
    /// </summary>
    /// <param name="startDate">Cover's start date</param>
    /// <param name="endDate">Cover's end date</param>
    /// <param name="coverType">Cover type</param>
    /// <returns>Calculated premium amount</returns>
    [HttpPost("/ComputePremium")]
    public ActionResult ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_premiumComputeService.ComputePremium(startDate, endDate, coverType));
    }

    /// <summary>
    /// Gets all created covers
    /// </summary>
    /// <returns>List of covers</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCoverResponse>>> GetAsync()
    {
        var covers = await _coversService.GetCoversAsync();

        return Ok(covers.Adapt<IEnumerable<GetCoverResponse>>());
    }

    /// <summary>
    /// Gets single cover by cover identifier
    /// </summary>
    /// <param name="id">Cover identifier</param>
    /// <returns>Cover object</returns>
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
            _logger.LogDebug("Cover by id {id} was not found", id);
            return NotFound();
        }
    }

    /// <summary>
    /// Creates cover by provided properties
    /// </summary>
    /// <param name="cover">Cover object</param>
    /// <returns>Created cover identifier</returns>
    [HttpPost]
    public async Task<ActionResult<string>> CreateAsync(CreateCoverRequest cover)
    {
        var validationResult = _validator.Validate(cover);
        if (!validationResult.IsValid)
        {
            _logger.LogDebug("The request is invalid, errors: {errors}", JsonConvert.SerializeObject(validationResult.Errors));
            return BadRequest(validationResult.Errors);
        }

        var coverId = await _coversService.CreateCoverAsync(cover.Adapt<Cover>());

        return Ok(coverId);
    }

    /// <summary>
    /// Deletes created cover
    /// </summary>
    /// <param name="id">Cover identifier</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        try
        {
            await _coversService.DeleteCoverAsync(id);
            return NoContent();
        }
        catch (CoverNotFoundException)
        {
            _logger.LogDebug("Cover by id {id} was not found", id);
            return NotFound();
        }
    }
}