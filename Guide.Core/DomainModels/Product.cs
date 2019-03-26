using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using Guide.Common;
using Guide.Common.Interfaces;
using Guide.Core.DomainEvents;
using Guide.Core.DomainEvents.Events.ProductEvents;

namespace Guide.Core.DomainModels
{
    public class Product : AuditableEntityBase<int>, IEventHandler<ProductPriceChangedDomainEvent>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int StockAmount { get; set; }
        public decimal ProductPrice { get; set; }
        public string PriceChangeDescription { get; set; }


        #region Actions
        public static Product CreateProduct(string name, string code, int stockAmount)
        {
            return new Product()
            {
                Name = name,
                Code = code,
                StockAmount = stockAmount
            };
        }

        public void ChangePrice(decimal productPrice)
        {
            Add(new ProductPriceChangedDomainEvent() { OldPrice = ProductPrice, NewPrice = productPrice, });
            ProductPrice = productPrice;

        }
        #endregion


        public void Handle(ProductPriceChangedDomainEvent evt)
        {
            // If necessary, do something for this entity
            PriceChangeDescription = $"Ürün fiyatı {evt.DateTimeEventOccurred} tarihinde {evt.OldPrice} --> {evt.NewPrice} olarak değişmiştir.";
        }
    }
}
