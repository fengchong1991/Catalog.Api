using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public interface IIntegrationEventHandler<in T> where T: IntegrationEvent
    {
        Task Handle(T @event);
    }
}
 