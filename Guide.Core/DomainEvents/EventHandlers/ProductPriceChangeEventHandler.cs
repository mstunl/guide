using Guide.Common.Interfaces;
using Guide.Core.DomainEvents.Events.ProductEvents;

namespace Guide.Core.DomainEvents.EventHandlers
{
    public class ProductPriceChangeEventHandler : IEventHandler<ProductPriceChangedDomainEvent>
    {
        public void Handle(ProductPriceChangedDomainEvent evt)
        {
            // Logging
            // Mailing

        }
    }

}

