using Guide.Common.Constants;

namespace Guide.Core.DomainEvents.Events.ProductEvents
{
    public class ProductPriceChangedDomainEvent : Event
    {
        public new static string EventKey = ProjectConstants.DomainEventQueue;
        public ProductPriceChangedDomainEvent() 
            : base(EventKey)
        {
            
        }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }

    }
}
