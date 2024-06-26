﻿namespace Claims.Application
{
    public interface IAuditerService
    {
        Task AuditClaimAsync(string id, string httpRequestType);
        Task AuditCoverAsync(string id, string httpRequestType);
    }
}