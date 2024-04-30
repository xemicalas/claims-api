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

        public void AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAuditEntity()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _auditContext.Add(claimAudit);
            _auditContext.SaveChanges();
        }

        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAuditEntity()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _auditContext.Add(coverAudit);
            _auditContext.SaveChanges();
        }
    }
}