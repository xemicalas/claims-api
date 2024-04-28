using Claims.Repositories.Repositories;

namespace Claims.Services
{
    public class AuditerService : IAuditerService
	{
        private readonly IAuditerRepository auditerRepository;

        public AuditerService(IAuditerRepository auditerRepository)
		{
            this.auditerRepository = auditerRepository;
        }

        public Task AuditClaim(string id, string httpRequestType)
        {
            return Task.CompletedTask;
        }

        public Task AuditCover(string id, string httpRequestType)
        {
            return Task.CompletedTask;
        }
    }
}

