using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Application.ApplicationEvents.Events.ProductEvents;
using Guide.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Guide.Application.ApplicationEvents.EventHandlers
{
    public class ProductPriceChangedApplicationEventHandler : IEventHandler<ProductPriceChangedApplicationEvent>
    {
        private readonly ILogger<ProductPriceChangedApplicationEventHandler> _logger;

        public ProductPriceChangedApplicationEventHandler(ILogger<ProductPriceChangedApplicationEventHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(ProductPriceChangedApplicationEvent evt)
        {
            // Do something
            _logger.LogInformation($"Product Price Changed, Product Code: {evt.ProductCode}, New Price: {evt.ProductPrice}");

        }
    }
}
