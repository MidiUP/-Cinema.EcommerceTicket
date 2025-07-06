using Cinema.EcommerceTicket.Domain.Exceptions;
using Cinema.EcommerceTicket.Domain.Shared;
using Cinema.EcommerceTicket.Infrastructure.RabbitMq.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

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
                        minInterval: TimeSpan.FromSeconds(30),   // Intervalo inicial
                        maxInterval: TimeSpan.FromSeconds(300),  // Intervalo máximo
                        intervalDelta: TimeSpan.FromSeconds(90)  // Fator de crescimento exponencial
                    );
                    r.Ignore(typeof(CinemaEcommerceTicketException));
                });

                AddConsumer<CreateTicketConsumer>(cfg, context, queueCreateEcommerceTicketName);
            });
        });

        services.AddHealthChecks()
            .AddRabbitMQ(async s =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitMqOptions.Host,
                    UserName = rabbitMqOptions.Username,
                    Password = rabbitMqOptions.Password,
                    Port = int.TryParse(rabbitMqOptions.Port, out var port) ? port : AmqpTcpEndpoint.UseDefaultPort
                };
                return await factory.CreateConnectionAsync();
            },
            name: "RabbitMQ",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["health"],
            timeout: TimeSpan.FromSeconds(2)
            );
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
            //e.UseConsumeFilter(typeof(ConsumeFilter<>), context);
            e.ConfigureConsumer<T>(context);
        });
    }

    private static string GetNameQueue(string queueName)
    {
        return $"{Domain.Shared.Constants.ENVIRONMENT}.{queueName}";
    }
}
