using Autofac;
using FluentValidation;
using Guide.Application.ApplicationServices;
using Guide.Application.CQRS;
using Guide.Application.CQRS.Command;
using Guide.Application.Data;
using Guide.Application.Data.Repositories;
using Guide.Application.Data.UnitOfWork;
using Guide.Application.ExceptionHandling;
using Guide.Application.MappingProfiles;
using Guide.Application.MediatrDecorators.Logging;
using Guide.Application.Utils;
using Guide.Application.ViewModel.CommandResponses.Product;
using Guide.Common.Interfaces;
using Guide.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Guide.API.Extensions;
using Guide.API.Model;
using Guide.Infrastructure.Messaging;
using Guide.Infrastructure.Messaging.RabbitMQ;
using ApplicationBuilderExtensions = Guide.API.Extensions.ApplicationBuilderExtensions;

namespace Guide.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddDbContext<GuideContext>(ServiceLifetime.Scoped);

            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IProductRepository, ProductRepository>();


            services.AddTransient<IUnitOfWork<int>, UnitOfWork<int>>();
            services.AddScoped<IEventBus>(provider => new RabbitMqEventPublisher("amqp://128.128.15.142"));
            services.AddScoped(typeof(IEventStorage<>), typeof(EventStorage<>));
            services.AddSingleton<RabbitMqEventSubscriber>();
            //services.AddTransient<IEventSubscriber, RabbitMqEventSubscriber>();

            

            services.AddAttributeHandlers();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ProductMappingProfile());
            });
            services.AddTransient<ProductManager>();

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(GlobalExceptionHandler));

            })
                .AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            AddEventHandlers(services);
            AddMediatr(services);
        }

      

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseRabbitSubscriber("amqp://128.128.15.142");
            app.UseStatusCodePages();
            AddNLog(env, loggerFactory);
            app.UseMvc();
        }




        private static void AddMediatr(IServiceCollection services)
        {
            const string applicationAssemblyName = "Guide.Application";
            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

            //services.AddScoped<IEventBus, EventBus>();

            var builder = new ContainerBuilder();

            // TODO: Bu koddaki inject işlemi ProductCreateCommandHandler daki[AuditLog] attribute ile yapılabilir.Ancak hata verdiği için yorum satırı yapıldı.Bakılacak!!
            services.AddTransient<IRequestHandler<ProductCreateCommand, ProductCreateView>>
            (
                provider => new AuditLoggingDecorator<ProductCreateCommand, ProductCreateView>(
                    provider.GetService<ILoggerFactory>(),
                    new ProductCreateCommand.ProductCreateCommandHandler(provider.GetService<IUnitOfWork<int>>())
                ));


            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MediatorPipeline<,>));
            services.AddScoped(typeof(IPreRequestHandler<>), typeof(RequestLogger<>));
            services.AddScoped(typeof(IRequestHandler<>), typeof(AuditLoggingDecorator<,>));




            // Bu injection yerine AuditLogAttribute yazıldı
            //services.AddTransient(typeof(IRequestHandler<,>), typeof(AuditLoggingDecorator<PointOfInterestCreateCommand, PointOfInterestDto>));
            //builder.RegisterType<AuditLoggingDecorator<ProductCreateCommand, ProductCreateView>>().As<IRequestHandler<IRequest<ProductCreateView>, ProductCreateView>>();

            services.AddMediatR();
        }
        private static void AddNLog(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();
        }



        private static void AddEventHandlers(IServiceCollection services)
        {
            var handlers = ApplicationBuilderExtensions.BuildHandlerConfigurations();
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
                services.AddSingleton(r.FromType, r.ToType);
            }
        }

    }
}
