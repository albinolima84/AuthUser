using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Sky.Auth.IoC.Extensions
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sky.Auth.Api V1");
              });

            return app;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Sky.Auth",
                    Version = "v1",
                    Description = "Api de cadastro e autenticação de usuário",
                    Contact = new Contact { Url = "https://github.com/albinolima84/AuthUser" }
                });
                options.AddSecurityDefinition("bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Gerar Token para efetuar requisição",
                        Name = "Authorization",
                        Type = "apiKey"
                    }
                );
            });

            return services;
        }
    }
}
