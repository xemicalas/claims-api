using System.Text.Json;
using Claims.Api.Contracts;
using Claims.Application;
using Claims.Domain.Contracts;
using Claims.Domain.Exceptions;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Claims.Api.Controllers;

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
    /// <response code="200">Returns calculated premium amount</response>
    [HttpPost("/ComputePremium")]
    public ActionResult ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        return Ok(_premiumComputeService.ComputePremium(startDate, endDate, coverType));
    }

    /// <summary>
    /// Gets all created covers
    /// </summary>
    /// <returns>List of covers</returns>
    /// <response code="200">Returns list of covers</response>
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
    /// <response code="200">Returns cover object</response>
    /// <response code="404">When cover is not found</response>
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
    /// <response code="200">When cover is successfully created</response>
    /// <response code="400">When the request is invalid</response>
    [HttpPost]
    public async Task<ActionResult<string>> CreateAsync(CreateCoverRequest cover)
    {
        var validationResult = _validator.Validate(cover);
        if (!validationResult.IsValid)
        {
            _logger.LogDebug("The request is invalid, errors: {errors}", JsonSerializer.Serialize(validationResult.Errors));
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
    /// <response code="204">When cover is successfully deleted</response>
    /// <response code="404">When cover is not found</response>
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