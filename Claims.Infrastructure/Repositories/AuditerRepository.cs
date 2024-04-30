using Claims.Infrastructure.Auditing;
using Claims.Infrastructure.Contracts;

namespace Claims.Infrastructure.Repositories
{
    public class AuditerRepository : IAuditerRepository
    {
        private readonly AuditContext _auditContext;

        public AuditerRepository(AuditContext auditContext)
        {
            _auditContext = auditContext;
        }

        public Task AuditClaimAsync(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAuditEntity()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _auditContext.Add(claimAudit);
            return _auditContext.SaveChangesAsync();
        }

        public Task AuditCoverAsync(string id, string httpRequestType)
        {
            var coverAudit = new CoverAuditEntity()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _auditContext.Add(coverAudit);
            return _auditContext.SaveChangesAsync();
        }
    }
}