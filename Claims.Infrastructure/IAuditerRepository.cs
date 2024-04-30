namespace Claims.Infrastructure
{
    public interface IAuditerRepository
    {
        Task AuditClaimAsync(string id, string httpRequestType);
        Task AuditCoverAsync(string id, string httpRequestType);
    }
}