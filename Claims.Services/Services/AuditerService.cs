using Claims.Repositories;

namespace Claims.Services
{
    public class AuditerService : IAuditerService
	{
        private readonly IAuditerRepository auditerRepository;

        public AuditerService(IAuditerRepository auditerRepository)
		{
            this.auditerRepository = auditerRepository;
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
            //auditerRepository.AuditClaim(id, httpRequestType);
        }

        private void AuditCover(string id, string httpRequestType)
        {
            //auditerRepository.AuditCover(id, httpRequestType);
        }
    }
}

