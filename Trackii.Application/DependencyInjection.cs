// Ruta: Trackii.Application/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using Trackii.Application.Interfaces;
using Trackii.Application.Services;

namespace Trackii.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ExecuteStepService>();
        services.AddScoped<CreateWorkOrderService>();
        services.AddScoped<CreateWipItemService>();
        services.AddScoped<CancelWorkOrderService>();
        services.AddScoped<IAuthService, AuthService>();


        return services;
    }
}
