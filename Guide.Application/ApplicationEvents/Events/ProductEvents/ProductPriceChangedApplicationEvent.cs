using System;
using Guide.Common.Constants;
using Guide.Common.Interfaces;
using Guide.Core;

namespace Guide.Application.ApplicationEvents.Events.ProductEvents
{
    public class ProductPriceChangedApplicationEvent : Event
    {
        public new static string EventKey = ProjectConstants.ApplicationEventQueue;

        public ProductPriceChangedApplicationEvent() 
            : base(EventKey)
        {

        }

        public string ProductCode { get; set; }
        public decimal ProductPrice { get; set; }

    }
}
