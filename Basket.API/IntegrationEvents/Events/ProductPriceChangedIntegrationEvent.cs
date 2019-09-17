using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.IntegrationEvents.Events
{
    public class ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; }
        public decimal NewPrice { get; }
        public decimal OldPrice { get; }

        public ProductPriceChangedIntegrationEvent(int proudctId, decimal newPrice, decimal oldPrice)
        {
            ProductId = proudctId;
            NewPrice = NewPrice;
            OldPrice = oldPrice;
        }
    }
}
