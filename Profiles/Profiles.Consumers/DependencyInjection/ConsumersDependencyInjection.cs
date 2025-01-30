using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Consumers.Consumers.OfficeConsumers;
using Profiles.Consumers.Consumers.SpecializationConsumers;
using Profiles.Consumers.Mapper;

namespace Services.Consumers.Consumers.DependencyInjection
{
    public static class ConsumersDependencyInjection
    {
        public static IServiceCollection AddConsumersDependencyInjection(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                AddOfficeConsumers(x);
                AddSpecializationConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureOfficeEndpoints(context, cfg);
                    ConfigureSpecializationEndpoints(context, cfg);
                });
            });

            services.AddAutoMapper(typeof(ConsumersMapping));

            return services;
        }

        private static void AddOfficeConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateOfficeConsumer>();
            x.AddConsumer<UpdateOfficeConsumer>();
        }

        private static void AddSpecializationConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateSpecializationConsumer>();
            x.AddConsumer<UpdateSpecializationConsumer>();
        }

        private static void ConfigureOfficeEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("office-created", e =>
            {
                e.ConfigureConsumer<CreateOfficeConsumer>(context);
            });
            cfg.ReceiveEndpoint("office-updated", e =>
            {
                e.ConfigureConsumer<UpdateOfficeConsumer>(context);
            });
        }

        private static void ConfigureSpecializationEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("specialization-created", e =>
            {
                e.ConfigureConsumer<CreateSpecializationConsumer>(context);
            });
            cfg.ReceiveEndpoint("specialization-updated", e =>
            {
                e.ConfigureConsumer<UpdateSpecializationConsumer>(context);
            });
        }
    }
}
