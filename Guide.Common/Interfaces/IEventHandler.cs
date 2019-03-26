using System;
using System.Collections.Generic;
using System.Text;

namespace Guide.Common.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent evt);
    }
}
