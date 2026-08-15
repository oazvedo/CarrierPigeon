using BirdMessage.Application.Externals.Interfaces;
using BirdMessage.Domain.Interfaces;
using BirdMessage.Infrastructure.Data;
using BirdMessage.Infrastructure.Data.Repositories;
using BirdMessage.Infrastructure.Messages.Consumers;
using BirdMessage.Infrastructure.Messages.Publishers;
using BirdMessage.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BirdMessage.Infrastructure;

public static class InfraModule
{
    public static IServiceCollection AddInfraModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<IBirdRepository, BirdRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        services.AddScoped<IMessageTrackingPublisher, MessageTrackingPublisher>();

        var rabbitMqSection = configuration.GetSection("RabbitMq");
        var rabbitMqHost = rabbitMqSection["Host"] ?? "localhost";
        var rabbitMqPort = rabbitMqSection.GetValue<ushort>("Port", 5672);
        var rabbitMqVirtualHost = rabbitMqSection["VirtualHost"] ?? "/";
        if (!rabbitMqVirtualHost.StartsWith('/'))
            rabbitMqVirtualHost = $"/{rabbitMqVirtualHost}";
        var rabbitMqUsername = rabbitMqSection["Username"] ?? "guest";
        var rabbitMqPassword = rabbitMqSection["Password"] ?? "guest";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageTrackingConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri($"rabbitmq://{rabbitMqHost}:{rabbitMqPort}{rabbitMqVirtualHost}"), h =>
                {
                    h.Username(rabbitMqUsername);
                    h.Password(rabbitMqPassword);
                });

                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
