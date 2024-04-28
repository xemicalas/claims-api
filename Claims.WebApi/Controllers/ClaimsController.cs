using Claims.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimsService _claimsService;
        private readonly IAuditerService _auditerService;

        public ClaimsController(ILogger<ClaimsController> logger, IClaimsService claimsService, IAuditerService auditerService)
        {
            _logger = logger;
            _claimsService = claimsService;
            _auditerService = auditerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> GetAsync()
        {
            var claims = await _claimsService.GetClaimsAsync();

            return Ok(claims.Adapt<IEnumerable<Claim>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetAsync(string id)
        {
            var claim = await _claimsService.GetClaimAsync(id);

            return Ok(claim.Adapt<Claim>());
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Claim claim)
        {
            claim.Id = Guid.NewGuid().ToString();
            await _claimsService.CreateClaimAsync(claim.Adapt<Domain.Claim>());
            await _auditerService.AuditClaim(claim.Id, "POST");

            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _auditerService.AuditClaim(id, "DELETE");
            await _claimsService.DeleteClaimAsync(id);
        }
    }
}