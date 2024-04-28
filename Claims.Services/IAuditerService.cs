﻿namespace Claims.Services
{
    public interface IAuditerService
    {
        Task AuditClaim(string id, string httpRequestType);
        Task AuditCover(string id, string httpRequestType);
    }
}