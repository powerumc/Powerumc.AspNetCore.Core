using System.Threading.Tasks;

namespace Powerumc.AspNetCore.Core.Domains
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent @event);
    }
}