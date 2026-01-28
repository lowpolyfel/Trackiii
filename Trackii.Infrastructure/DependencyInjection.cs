using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trackii.Application.Interfaces;
using Trackii.Infrastructure.Persistence;
using Trackii.Infrastructure.Repositories;
using Trackii.Infrastructure.Services;

namespace Trackii.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<TrackiiDbContext>(options =>
        {
            var cs = configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(cs, ServerVersion.AutoDetect(cs));
        });

        // Unit of Work & Clock
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IClock, SystemClock>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IWipItemRepository, WipItemRepository>();
        services.AddScoped<IWipStepExecutionRepository, WipStepExecutionRepository>();
        services.AddScoped<IScanEventRepository, ScanEventRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IRouteStepRepository, RouteStepRepository>();

        // Auth
        services.AddScoped<IPasswordHasher, PasswordHasherAdapter>();

        return services;
    }
}
