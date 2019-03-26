using System;
using System.Collections.Generic;
using System.Text;

namespace Guide.Common.Interfaces
{
    public interface IEvent
    {
        string EventKey { get; }
        DateTime DateTimeEventOccurred { get; }
    }
}
