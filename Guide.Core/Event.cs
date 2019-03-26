using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common.Interfaces;

namespace Guide.Core
{
    public class Event : IEvent
    {
   
        public string EventKey { get; }
        public DateTime DateTimeEventOccurred { get; }

        public Event(string eventKey)
        {
            DateTimeEventOccurred = DateTime.Now;
            EventKey = eventKey;
        }
        public Event()
        {

        }
    }
}
