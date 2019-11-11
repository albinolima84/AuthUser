using Microsoft.Extensions.DependencyInjection;
using Sky.Auth.Data.Connection;
using Sky.Auth.Data.Repositories;
using Sky.Auth.Domain.Interfaces;

namespace Sky.Auth.IoC.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IConnect, Connect>();
            services.AddSingleton<IAuthRepository, AuthRepository>();

            return services;
        }
    }
}
