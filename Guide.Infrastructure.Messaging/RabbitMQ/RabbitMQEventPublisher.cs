using Guide.Common.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Guide.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqEventPublisher : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqEventPublisher(string uri)
        {
            var factory = new ConnectionFactory()
            {
                //Uri = new Uri(uri),
                Uri = new Uri("amqp://hradmin:HumanR15672@128.128.15.142:5672"),
                UserName = "hradmin",
                Password = "HumanR15672"
                //Port = 15672
            };
            this._connection = factory.CreateConnection();
            this._channel = _connection.CreateModel();



        }
        public void Dispose()
        {
            this._channel.Dispose();
            this._connection.Dispose();
        }

        public void Publish<T>(T @event) where T : IEvent
        {

            //this._channel.QueueDeclare(queue: @event.EventKey, durable: false, exclusive: false, autoDelete: false);

            var json = JsonConvert.SerializeObject(@event, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var bytes = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            _channel.BasicPublish(exchange: "", routingKey: @event.EventKey, basicProperties: properties, body: bytes);
        }
    }
}
