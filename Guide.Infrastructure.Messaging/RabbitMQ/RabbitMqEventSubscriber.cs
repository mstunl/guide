using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using Guide.Common.Interfaces;
using Guide.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Guide.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqEventSubscriber : IEventSubscriber
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _provider;
        public RabbitMqEventSubscriber(string uri, IServiceProvider provider)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://hradmin:HumanR15672@128.128.15.142:5672"),
                UserName = "hradmin",
                Password = "HumanR15672"
            };
            this._connection = factory.CreateConnection();
            this._channel = _connection.CreateModel();
            this._provider = provider;
        }


        public void Dispose()
        {
            this._channel.Dispose();
            this._connection.Dispose();
        }

        public void Subscribe<T>(T @event) where T : IEvent
        {
            this._channel.QueueDeclare(queue: @event.EventKey, durable: true, exclusive: false, autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(_channel);


            var instance = _provider.GetService<IEventHandler<T>>();

            if (instance == null)
            {
                return;
            }

            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);

                var cmd = JsonConvert.DeserializeObject<T>(message);
                Console.WriteLine("[x] Receive New Event: {0}", @event.EventKey);
                Console.WriteLine("[x] Event Parameters: {0}", message);
                try
                {
                    instance.Handle(cmd);
                }
                catch
                {

                }
                Console.WriteLine("[x] Event Handler Completed");
                _channel.BasicAck(e.DeliveryTag, false);
            };


            _channel.BasicConsume(queue: @event.EventKey, autoAck: false, consumer: consumer);
        }


        public void Unsubscribe()
        {
            _connection.Close();
        }
    }
}
