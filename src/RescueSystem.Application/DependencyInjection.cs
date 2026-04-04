using Microsoft.Extensions.DependencyInjection;
using RescueSystem.Application.Services;

namespace RescueSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
