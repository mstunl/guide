using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Guide.API.Model;
using Guide.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Test
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }

        private static void AddEventHandlers(IServiceCollection services)
        {
            var handlers = BuildHandlerConfigurations();
            foreach (var handler in handlers)
            {
                RegisterEventHandlers(services, handler.LibraryName);
            }
        }
        private static void RegisterEventHandlers(IServiceCollection services, string libraryName)
        {
            bool IsEventHandler(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>);

            var commandHandlers = Assembly.Load(libraryName).GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(IsEventHandler))
                .ToList();

            var registerSource = commandHandlers.Select(h => new { FromType = h.GetInterfaces().First(IsEventHandler), ToType = h }).ToList();

            foreach (var r in registerSource)
            {
                services.AddScoped(r.FromType, r.ToType);
            }
        }
        private static IEnumerable<HandlerConfigurationDto> BuildHandlerConfigurations()
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
