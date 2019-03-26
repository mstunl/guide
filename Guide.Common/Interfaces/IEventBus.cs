using System;

namespace Guide.Common.Interfaces
{
    public interface IEventBus : IDisposable
    {
        void Publish<T>(T @event) where T : IEvent;
    }
}
