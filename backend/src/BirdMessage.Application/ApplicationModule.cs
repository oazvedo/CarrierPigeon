using BirdMessage.Application.Services;
using BirdMessage.Application.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace BirdMessage.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IDeliveryEstimationService, DeliveryEstimationService>();
        services.AddScoped<IBirdService, BirdService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}
