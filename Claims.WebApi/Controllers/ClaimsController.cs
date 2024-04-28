using Claims.Services;
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
        public Task<IEnumerable<Claim>> GetAsync()
        {
            return _claimsService.GetClaimsAsync();
        }

        [HttpGet("{id}")]
        public async Task<Claim> GetAsync(string id)
        {
            return _claimsService.GetClaimAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Claim claim)
        {
            claim.Id = Guid.NewGuid().ToString();
            await _claimsService.CreateClaimAsync(claim);
            await _auditerService.AuditClaim(claim.Id, "POST");
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _auditerService.AuditClaim(id, "DELETE");
            return _claimsService.DeleteClaimAsync(id);
        }
    }
}