using System;

namespace Powerumc.AspNetCore.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RegisterAttribute : Attribute
    {
        public Type RegistrationType { get; }
        public ServiceLifetime ServiceLifetime { get; } = ServiceLifetime.Singleton;

        public RegisterAttribute(Type registrationType)
        {
            this.RegistrationType = registrationType;
        }

        public RegisterAttribute(Type registrationType, ServiceLifetime serviceLifetime) : this(registrationType)
        {
            this.ServiceLifetime = serviceLifetime;
        }
    }
}