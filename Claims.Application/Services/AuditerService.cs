using Claims.Infrastructure;

namespace Claims.Application.Services
{
    public class AuditerService : IAuditerService
    {
        private readonly IAuditerRepository _auditerRepository;

        public AuditerService(IAuditerRepository auditerRepository)
        {
            _auditerRepository = auditerRepository;
        }

        public Task AuditClaimAsync(string id, string httpRequestType)
        {
            // For improvement: could use asynchronouse messaging platform like RabbitMQ, NSQ, etc..
            Task.Factory.StartNew(() => AuditClaim(id, httpRequestType));
            return Task.CompletedTask;
        }

        public Task AuditCoverAsync(string id, string httpRequestType)
        {
            // For improvement: could use asynchronouse messaging platform like RabbitMQ, NSQ, etc..
            Task.Factory.StartNew(() => AuditCover(id, httpRequestType));
            return Task.CompletedTask;
        }

        private void AuditClaim(string id, string httpRequestType)
        {
            _auditerRepository.AuditClaim(id, httpRequestType);
        }

        private void AuditCover(string id, string httpRequestType)
        {
            _auditerRepository.AuditCover(id, httpRequestType);
        }
    }
}

