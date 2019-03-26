using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Guide.Application.CQRS;
using Guide.Application.MediatrDecorators.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Guide.Application.Utils
{
    public static class AttributeRegistiration
    {
        public static void AddAttributeHandlers(this IServiceCollection services)
        {
            List<Type> handlerTypes = typeof(MediatorPipeline<,>).Assembly.GetTypes()
                   .Where(x => x.GetInterfaces().Any(IsHandlerInterface))
                   .Where(x => x.Name.EndsWith("Handler"))
                   .ToList();

            foreach (Type type in handlerTypes)
            {
                AddAttributeHandler(services, type);
            }
        }

        private static void AddAttributeHandler(IServiceCollection services, Type type)
        {
            object[] attributes = type.GetCustomAttributes(false);

            if (!attributes.Any())
                return;

            var pipeline = attributes
                .Select(ToDecorator)
                .Reverse()
                .ToList();

            var interfaces = type.GetInterfaces();
            Type interfaceType = interfaces.Single(IsHandlerInterface);
            Func<IServiceProvider, object> factory = BuildPipeline(pipeline, interfaceType);

            services.AddTransient(interfaceType, factory);
        }

        private static Func<IServiceProvider, object> BuildPipeline(IEnumerable<Type> pipeline, Type interfaceType)
        {
            var ctors = pipeline
               .Select(x =>
               {
                   Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                   return type.GetConstructors().Single();
               })
               .ToList();

            Func<IServiceProvider, object> func = provider =>
            {
                object current = null;

                foreach (ConstructorInfo ctor in ctors)
                {
                    List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();

                    object[] parameters = GetParameters(parameterInfos, current, provider);

                    current = ctor.Invoke(parameters);
                }

                return current;
            };

            return func;
        }

        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, provider);
            }

            return result;
        }
        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            var parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType))
                return current;

            var service = provider.GetService(parameterType);
            if (service != null)
                return service;

            throw new ArgumentException($"Type {parameterType} not found");
        }

        private static Type ToDecorator(object attribute)
        {
            var type = attribute.GetType();

            if (type == typeof(AuditLogAttribute))
                return typeof(AuditLoggingDecorator<,>);

            // other attributes go here

            throw new ArgumentException(attribute.ToString());
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            var result = typeDefinition == typeof(IRequestHandler<,>);
            return result;
        }
    }
}
