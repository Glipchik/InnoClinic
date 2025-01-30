using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Offices.Consumers.DoctorConsumers;
using Offices.Consumers.Mapper;
using Offices.Consumers.ReceptionistConsumers;
using Offices.Data.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Consumers.DependencyInjection
{
    public static class ConsumersDependencyInjection
    {
        public static IServiceCollection AddConsumersDependencyInjection(this IServiceCollection services)
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
            cfg.ReceiveEndpoint("doctor-created", e =>
            {
                e.ConfigureConsumer<CreateDoctorConsumer>(context);
            });
            cfg.ReceiveEndpoint("doctor-updated", e =>
            {
                e.ConfigureConsumer<UpdateDoctorConsumer>(context);
            });
        }

        private static void ConfigureReceptionistEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("receptionist-created", e =>
            {
                e.ConfigureConsumer<CreateReceptionistConsumer>(context);
            });
            cfg.ReceiveEndpoint("receptionist-updated", e =>
            {
                e.ConfigureConsumer<UpdateReceptionistConsumer>(context);
            });
            cfg.ReceiveEndpoint("receptionist-deleted", e =>
            {
                e.ConfigureConsumer<DeleteReceptionistConsumer>(context);
            });
        }
    }
}
