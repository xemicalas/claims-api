using Claims.Domain.Contracts;
using Claims.Domain.Contracts.Exceptions;
using Claims.Services;
using Claims.WebApi.Contracts;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimsService _claimsService;
        private readonly IValidator<CreateClaimRequest> _validator;

        public ClaimsController(ILogger<ClaimsController> logger, IClaimsService claimsService, IValidator<CreateClaimRequest> validator)
        {
            _logger = logger;
            _claimsService = claimsService;
            _validator = validator;
        }

        /// <summary>
        /// Gets all awailable created claims
        /// </summary>
        /// <returns>List of claims</returns>
        /// <response code="200">Returns list of claims</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetClaimResponse>>> GetAsync()
        {
            var claims = await _claimsService.GetClaimsAsync();
            return Ok(claims.Adapt<IEnumerable<GetClaimResponse>>());
        }

        /// <summary>
        /// Gets claim by claim identifier
        /// </summary>
        /// <param name="id">Claim identifier</param>
        /// <returns>Claim response</returns>
        /// <response code="200">Returns claim response</response>
        /// <response code="404">When the claim is not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetClaimResponse>> GetAsync(string id)
        {
            try
            {
                var claim = await _claimsService.GetClaimAsync(id);
                return Ok(claim.Adapt<GetClaimResponse>());
            }
            catch (ClaimNotFoundException)
            {
                _logger.LogDebug("Claim by id {id} was not found", id);
                return NotFound();
            }
        }

        /// <summary>
        /// Creates claim by provided properties
        /// </summary>
        /// <param name="claim">Claim object</param>
        /// <returns>Created claim identifier</returns>
        /// <response code="200">Returns created claim identifier</response>
        /// <response code="400">When the request is invalid</response>
        [HttpPost]
        public async Task<ActionResult<string>> CreateAsync(CreateClaimRequest claim)
        {
            var validationResult = await _validator.ValidateAsync(claim);
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("The request is invalid, errors: {errors}", JsonConvert.SerializeObject(validationResult.Errors));
                return BadRequest(validationResult.Errors);
            }

            var claimId = await _claimsService.CreateClaimAsync(claim.Adapt<Claim>());

            return Ok(claimId);
        }

        /// <summary>
        /// Deletes created claim
        /// </summary>
        /// <param name="id">Claim identifier</param>
        /// <returns></returns>
        /// <response code="204">When claim is successfully deleted</response>
        /// <response code="404">When claim is not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                await _claimsService.DeleteClaimAsync(id);
                return NoContent();
            }
            catch (ClaimNotFoundException)
            {
                _logger.LogDebug("Claim by id {id} was not found", id);
                return NotFound();
            }
        }
    }
}