using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sky.Auth.CrossCutting.Options;

namespace Sky.Auth.IoC.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(option =>
            {
                option.ConnectionString = configuration.GetSection("MongoOptions:ConnectionString").Value;
                option.Database = configuration.GetSection("MongoOptions:Database").Value;
                option.Collection = configuration.GetSection("MongoOptions:Collection").Value;
            });
            
            return services;
        }
    }
}
