using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guide.Common.Interfaces
{
    public interface IEventSubscriber
    {
        void Subscribe<T>(T domainEvent) where T : IEvent;
    }
}
