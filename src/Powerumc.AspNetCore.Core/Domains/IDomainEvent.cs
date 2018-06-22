using System;

namespace Powerumc.AspNetCore.Core.Domains
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime CreateDate { get; }
    }
}