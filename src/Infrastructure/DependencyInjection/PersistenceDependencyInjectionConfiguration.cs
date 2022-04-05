using Core.Persistence;
using Infrastructure.Persistence;
using Marten;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class PersistenceDependencyInjectionConfiguration
    {
        public static IServiceCollection AddInfrastructureRepositories(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var config = configuration.GetSection("postgresql").Get<PostgreSqlOptions>((a) => a.BindNonPublicProperties = true);

            services.AddDbContext<UserZoomContext>(options => options.UseNpgsql(config.ConnectionString));

            //services.AddMarten(config.ConnectionString);

            return (services);
        }
    }
}