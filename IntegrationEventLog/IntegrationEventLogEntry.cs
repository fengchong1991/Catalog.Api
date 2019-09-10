using EventBus.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;


namespace IntegrationEventLog
{
    [Table("IntegrationEventLog")]
    public class IntegrationEventLogEntry
    {
        public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }

        public Guid EventId { get; set; }
        public string EventTypeName { get; set; }
        
        public EventStateEnum State{ get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; set; }
        public string Content { get; set; }
        public string TransactionId { get; set; }

        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();

        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
            return this;
        }
    }
}
