using Microsoft.EntityFrameworkCore;

namespace Claims.Auditing
{
    public class AuditContext : DbContext
    {
        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
        }
        public DbSet<ClaimAuditEntity> ClaimAudits { get; set; }
        public DbSet<CoverAuditEntity> CoverAudits { get; set; }
    }
}
