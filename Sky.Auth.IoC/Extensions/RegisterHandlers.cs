using Sky.Auth.Command.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Sky.Auth.IoC.Extensions
{
    public static class HandlerExtensions
    {
        public static IServiceCollection RegisterHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateUserHandler).Assembly);

            return services;
        }
    }
}
