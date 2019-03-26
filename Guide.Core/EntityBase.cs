using System.Collections.Generic;

namespace Guide.Core
{
    public class EntityBase<TKey>
    {
        public TKey Id { get; protected  set; }

        private readonly List<Event> _domainEvents;

        public EntityBase()
        {
            _domainEvents = new List<Event>();
        }
        
        public IEnumerable<Event> GetUncommittedEvents()
        {
            return _domainEvents;
        }


        public void Raise(Event @event)
        {
            dynamic d = this;
            d.Handle(Converter.ChangeTo(@event, @event.GetType()));

        }
        public void Add(Event @event)
        {
            _domainEvents.Add(@event);
        }

        public void RaiseAndAdd(Event @event)
        {
            dynamic d = this;
            d.Handle(Converter.ChangeTo(@event, @event.GetType()));
            _domainEvents.Add(@event);
        }
    }
}
