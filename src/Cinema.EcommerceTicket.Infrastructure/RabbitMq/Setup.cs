using Cinema.EcommerceTicket.Domain.Exceptions;
using Cinema.EcommerceTicket.Domain.Shared;
using Cinema.EcommerceTicket.Infrastructure.RabbitMq.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cinema.EcommerceTicket.Infrastructure.RabbitMq;

public static class Setup
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>()!;

        var queueCreateEcommerceTicketName = GetNameQueue(rabbitMqOptions.QueueCreateEcommerceTicketName);

        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateTicketConsumer>();
            ConfigureHealthCheck(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                
                cfg.Host(rabbitMqOptions.Host, "/", h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });

                cfg.UseConsumeFilter(typeof(GlobalExceptionFilter<>), context);

                cfg.UseMessageRetry(r =>
                {
                    r.Exponential(
                        retryLimit: 3,                  // Número de tentativas
                        minInterval: TimeSpan.FromSeconds(10),   // Intervalo inicial
                        maxInterval: TimeSpan.FromSeconds(60),  // Intervalo máximo
                        intervalDelta: TimeSpan.FromSeconds(25)  // Fator de crescimento exponencial
                    );
                    r.Ignore(typeof(CinemaEcommerceTicketException));
                });

                AddConsumer<CreateTicketConsumer>(cfg, context, queueCreateEcommerceTicketName);

            });
        });
    }

    private static void ConfigureHealthCheck(IBusRegistrationConfigurator x)
    {
        x.ConfigureHealthCheckOptions(options =>
        {
            options.Name = "masstransit";
            options.MinimalFailureStatus = HealthStatus.Unhealthy;
            options.Tags.Add("health");
        });
    }

    private static void AddConsumer<T>(IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context,
        string queue) where T : class, IConsumer
    {

        cfg.ReceiveEndpoint(queue, e =>
        {
            e.ConfigureConsumer<T>(context);
        });
    }

    private static string GetNameQueue(string queueName)
    {
        return $"{Constants.ENVIRONMENT}.{queueName}";
    }
}
