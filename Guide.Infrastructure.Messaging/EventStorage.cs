using System;
using System.Collections.Generic;
using System.Text;
using Guide.Common;
using Guide.Common.Interfaces;
using Guide.Core;
using Guide.Core.Interfaces;

namespace Guide.Infrastructure.Messaging
{
    public class EventStorage<TKey> : IEventStorage<TKey>
    {
        private readonly IEventBus _eventBus;

        public EventStorage(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Save(EntityBase<TKey> entity)
        {
            var uncommittedEvents = entity.GetUncommittedEvents();
            foreach (var @event in uncommittedEvents)
            {
                var destEvent = Converter.ChangeTo(@event, @event.GetType());
                _eventBus.Publish(destEvent);
            }
        }
    }
}
