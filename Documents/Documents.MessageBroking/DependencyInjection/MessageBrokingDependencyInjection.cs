﻿using Documents.MessageBroking.Consumers.AccountConsumers;
using Documents.MessageBroking.Consumers.AppointmentConsumers;
using Documents.MessageBroking.Consumers.DoctorConsumers;
using Documents.MessageBroking.Consumers.PatientConsumers;
using Documents.MessageBroking.Consumers.ServiceCategoryConsumers;
using Documents.MessageBroking.Consumers.ServiceConsumers;
using Documents.MessageBroking.Consumers.SpecializationConsumers;
using Documents.MessageBroking.Producers.Abstractions.ResultProducers;
using Documents.MessageBroking.Producers.ResultProducers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Results.MessageBroking.Mapper;

namespace Results.MessageBroking.DependencyInjection
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
                AddAccountConsumers(x);
                AddAppointmentConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigurePatientEndpoints(context, cfg);
                    ConfigureDoctorEndpoints(context, cfg);
                    ConfigureServiceCategoryEndpoints(context, cfg);
                    ConfigureServiceEndpoints(context, cfg);
                    ConfigureSpecializationEndpoints(context, cfg);
                    ConfigureAccountEndpoints(context, cfg);
                    ConfigureAppointmentEndpoints(context, cfg);
                });
            });

            services.AddAutoMapper(typeof(ConsumersMapping));
            services.AddAutoMapper(typeof(EventMapping));

            services.AddScoped<IResultProducer, ResultProducer>();

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
            x.AddConsumer<DeactivateDoctorConsumer>();
        }

        private static void AddServiceCategoryConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateServiceCategoryConsumer>();
            x.AddConsumer<UpdateServiceCategoryConsumer>();
            x.AddConsumer<DeleteServiceCategoryConsumer>();
        }

        private static void AddAppointmentConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateAppointmentConsumer>();
            x.AddConsumer<UpdateAppointmentConsumer>();
            x.AddConsumer<DeleteAppointmentConsumer>();
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

        private static void AddAccountConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<CreateAccountConsumer>();
            x.AddConsumer<DeleteAccountConsumer>();
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

        private static void ConfigureAccountEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("account-created", e =>
            {
                e.ConfigureConsumer<CreateAccountConsumer>(context);
            });
            cfg.ReceiveEndpoint("account-deleted", e =>
            {
                e.ConfigureConsumer<DeleteAccountConsumer>(context);
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

        private static void ConfigureAppointmentEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("appointment-created", e =>
            {
                e.ConfigureConsumer<CreateAppointmentConsumer>(context);
            });
            cfg.ReceiveEndpoint("appointment-updated", e =>
            {
                e.ConfigureConsumer<UpdateAppointmentConsumer>(context);
            });
            cfg.ReceiveEndpoint("appointment-deleted", e =>
            {
                e.ConfigureConsumer<DeleteAppointmentConsumer>(context);
            });
        }
    }
}
