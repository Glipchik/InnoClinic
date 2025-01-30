using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Services.Consumers.Consumers.DoctorConsumers;
using Services.Consumers.Mapper;

namespace Services.Consumers.Consumers.DependencyInjection
{
    public static class ConsumersDependencyInjection
    {
        public static IServiceCollection AddConsumersDependencyInjection(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                AddDoctorConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureDoctorEndpoints(context, cfg);
                });
            });

            services.AddAutoMapper(typeof(ConsumersMapping));

            return services;
        }

        private static void AddDoctorConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateDoctorConsumer>();
            x.AddConsumer<UpdateDoctorConsumer>();
        }

        private static void ConfigureDoctorEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("doctor-created", e =>
            {
                e.ConfigureConsumer<CreateDoctorConsumer>(context);
            });
            cfg.ReceiveEndpoint("doctor-updated", e =>
            {
                e.ConfigureConsumer<UpdateDoctorConsumer>(context);
            });
        }
    }
}
