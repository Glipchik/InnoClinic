using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Offices.Consumers.Mapper;
using Offices.MessageBroking.Consumers.DoctorConsumers;
using Offices.MessageBroking.Consumers.ReceptionistConsumers;
using Offices.MessageBroking.Mapper;
using Offices.MessageBroking.Producers;
using Offices.MessageBroking.Producers.Abstractions;

namespace Offices.MessageBroking.DependencyInjection
{
    public static class MessageBrokingDependencyInjection
    {
        public static IServiceCollection AddMessageBrokingDependencyInjection(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                AddDoctorConsumers(x);
                AddReceptionistConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureDoctorEndpoints(context, cfg);
                    ConfigureReceptionistEndpoints(context, cfg);
                });
            });

            services.AddAutoMapper(typeof(ConsumersMapping));
            services.AddAutoMapper(typeof(ProducersMapping));

            services.AddScoped<IOfficeProducer, OfficeProducer>();

            return services;
        }

        private static void AddDoctorConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateDoctorConsumer>();
            x.AddConsumer<UpdateDoctorConsumer>();
        }

        private static void AddReceptionistConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateReceptionistConsumer>();
            x.AddConsumer<UpdateReceptionistConsumer>();
            x.AddConsumer<DeleteReceptionistConsumer>();
        }

        private static void ConfigureDoctorEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("offices-doctor-created", e =>
            {
                e.ConfigureConsumer<CreateDoctorConsumer>(context);
            });
            cfg.ReceiveEndpoint("offices-doctor-updated", e =>
            {
                e.ConfigureConsumer<UpdateDoctorConsumer>(context);
            });
        }

        private static void ConfigureReceptionistEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("offices-receptionist-created", e =>
            {
                e.ConfigureConsumer<CreateReceptionistConsumer>(context);
            });
            cfg.ReceiveEndpoint("offices-receptionist-updated", e =>
            {
                e.ConfigureConsumer<UpdateReceptionistConsumer>(context);
            });
            cfg.ReceiveEndpoint("offices-receptionist-deleted", e =>
            {
                e.ConfigureConsumer<DeleteReceptionistConsumer>(context);
            });
        }
    }
}
