using Appointments.Consumers.Consumers.DoctorConsumers;
using Appointments.Consumers.Consumers.PatientConsumers;
using Appointments.Consumers.Consumers.ServiceCategoryConsumers;
using Appointments.Consumers.Consumers.ServiceConsumers;
using Appointments.Consumers.Consumers.SpecializationConsumers;
using Appointments.Consumers.Mapper;
using Appointments.MessageBroking.Producers.Abstractions.AppointmentProducers;
using Appointments.MessageBroking.Producers.AppointmentProducers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.MessageBroking.DependencyInjection
{
    public static class MessageBrokingDependencyInjection
    {
        public static IServiceCollection AddMessageBrokingDependencyInjection(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                AddPatientConsumers(x);
                AddDoctorConsumers(x);
                AddServiceCategoryConsumers(x);
                AddServiceConsumers(x);
                AddSpecializationConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigurePatientEndpoints(context, cfg);
                    ConfigureDoctorEndpoints(context, cfg);
                    ConfigureServiceCategoryEndpoints(context, cfg);
                    ConfigureServiceEndpoints(context, cfg);
                    ConfigureSpecializationEndpoints(context, cfg);
                });
            });

            services.AddAutoMapper(typeof(ConsumersMapping));
            services.AddAutoMapper(typeof(EventMapping));

            services.AddScoped<IAppointmentProducer, AppointmentProducer>();

            return services;
        }

        private static void AddPatientConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreatePatientConsumer>();
            x.AddConsumer<UpdatePatientConsumer>();
            x.AddConsumer<DeletePatientConsumer>();
        }

        private static void AddDoctorConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateDoctorConsumer>();
            x.AddConsumer<UpdateDoctorConsumer>();
        }

        private static void AddServiceCategoryConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateServiceCategoryConsumer>();
            x.AddConsumer<UpdateServiceCategoryConsumer>();
            x.AddConsumer<DeleteServiceCategoryConsumer>();
        }

        private static void AddServiceConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateServiceConsumer>();
            x.AddConsumer<UpdateServiceConsumer>();
        }

        private static void AddSpecializationConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateSpecializationConsumer>();
            x.AddConsumer<UpdateSpecializationConsumer>();
        }

        private static void ConfigurePatientEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("patient-created", e =>
            {
                e.ConfigureConsumer<CreatePatientConsumer>(context);
            });
            cfg.ReceiveEndpoint("patient-updated", e =>
            {
                e.ConfigureConsumer<UpdatePatientConsumer>(context);
            });
            cfg.ReceiveEndpoint("patient-deleted", e =>
            {
                e.ConfigureConsumer<DeletePatientConsumer>(context);
            });
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

        private static void ConfigureServiceEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("service-created", e =>
            {
                e.ConfigureConsumer<CreateServiceConsumer>(context);
            });
            cfg.ReceiveEndpoint("service-updated", e =>
            {
                e.ConfigureConsumer<UpdateServiceConsumer>(context);
            });
        }

        private static void ConfigureServiceCategoryEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("service-category-created", e =>
            {
                e.ConfigureConsumer<CreateServiceCategoryConsumer>(context);
            });
            cfg.ReceiveEndpoint("service-category-updated", e =>
            {
                e.ConfigureConsumer<UpdateServiceCategoryConsumer>(context);
            });
            cfg.ReceiveEndpoint("service-category-deleted", e =>
            {
                e.ConfigureConsumer<DeleteServiceCategoryConsumer>(context);
            });
        }
    }
}
