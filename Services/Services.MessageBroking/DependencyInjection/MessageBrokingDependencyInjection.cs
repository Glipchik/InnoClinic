using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Services.Consumers.Consumers.DoctorConsumers;
using Services.Consumers.Mapper;
using Services.MessageBroking.Mapper;
using Services.MessageBroking.Producers;
using Services.MessageBroking.Producers.Abstractions;

namespace Services.Consumers.Consumers.DependencyInjection
{
    public static class MessageBrokingDependencyInjection
    {
        public static IServiceCollection AddMessageBrokingDependencyInjection(this IServiceCollection services)
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
            services.AddAutoMapper(typeof(ProducersMapping));

            services.AddScoped<IServiceCategoryProducer, ServiceCategoryProducer>();
            services.AddScoped<IServiceProducer, ServiceProducer>();
            services.AddScoped<IDoctorProducer, DoctorProducer>();
            services.AddScoped<ISpecializationProducer, SpecializationProducer>();

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
