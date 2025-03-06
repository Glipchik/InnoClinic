using Authorization.MessageBroking.Consumers.AccountConsumers;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.MessageBroking.DependencyInjection
{
    public static class MessageBrokingDependencyInjection
    {
        public static IServiceCollection AddMessageBrokingDependencyInjection(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                AddAccountConsumers(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    ConfigureAccountEndpoints(context, cfg);
                });
            });

            return services;
        }

        private static void AddAccountConsumers(IBusRegistrationConfigurator x)
        {
            x.AddConsumer<DeleteAccountConsumer>();
        }

        private static void ConfigureAccountEndpoints(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.ReceiveEndpoint("authorization-account-deleted", e =>
            {
                e.ConfigureConsumer<DeleteAccountConsumer>(context);
            });
        }
    }
}
