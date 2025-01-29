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
                x.AddConsumer<CreateDoctorConsumer>();
                x.AddConsumer<UpdateDoctorConsumer>();

                x.AddConsumer<CreateReceptionistConsumer>();
                x.AddConsumer<UpdateReceptionistConsumer>();
                x.AddConsumer<DeleteReceptionistConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("doctor-created", e =>
                    {
                        e.ConfigureConsumer<CreateDoctorConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("doctor-updated", e =>
                    {
                        e.ConfigureConsumer<UpdateDoctorConsumer>(context);
                    });

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
                });
            });

            services.AddAutoMapper(typeof(ConsumersMapping));

            return services;
        }
    }
}
