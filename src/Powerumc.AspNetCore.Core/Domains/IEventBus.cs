using System;

namespace Powerumc.AspNetCore.Core.Domains
{
    public interface IEventBus
    {
        void Publish(IDomainEvent @event);
        void Subscribe(Type eventType, Type eventHandlerType);
    }
}