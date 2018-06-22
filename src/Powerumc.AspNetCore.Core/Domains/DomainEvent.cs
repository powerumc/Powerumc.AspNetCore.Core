using System;

namespace Powerumc.AspNetCore.Core.Domains
{
    public class DomainEvent : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime CreateDate { get; } = DateTime.Now;
    }
}