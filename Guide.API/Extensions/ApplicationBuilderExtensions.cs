using Guide.API.Model;
using Guide.Infrastructure.Messaging.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Guide.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static RabbitMqEventSubscriber Subscriber { get; set; }

        public static IApplicationBuilder UseRabbitSubscriber(this IApplicationBuilder app, string uri)
        {
            Subscriber = new RabbitMqEventSubscriber(uri, app.ApplicationServices);
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);
            return app;
        }
        
        private static void OnStarted()
        {
            var handlersDto = BuildHandlerConfigurations();

            foreach (var handlerDto in handlersDto)
            {
                RegisterAndStart(handlerDto.LibraryName);
            }

        }

        private static void OnStopping() 
        {
            Subscriber.Unsubscribe();
        }
        
        private static void RegisterAndStart(string libraryName)
        {
            var assembly = Assembly.Load(libraryName);

            var allEvents = assembly.GetExportedTypes().Where(p => p.GetInterface("IEvent") != null && p.Name != "Event");

            foreach (var @event in allEvents)
            {
                var register = Subscriber;
                {
                    var registerMethod = register.GetType().GetMethod("Subscribe");
                    var cmd = Activator.CreateInstance(@event);
                    Console.WriteLine($"Find event {@event.FullName}.");
                    registerMethod.MakeGenericMethod(@event).Invoke(register, new object[1] { cmd });

                }
            }
        }
        
        public static IEnumerable<HandlerConfigurationDto> BuildHandlerConfigurations()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            var sHandlers = configuration.GetSection("handlers").GetChildren();

            return sHandlers.Select(sHandler => new HandlerConfigurationDto { Name = sHandler["name"], LibraryName = sHandler["libraryName"] }).ToList();
        }
    }

}
