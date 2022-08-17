using Application.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EkmekBotDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("EkmekBotDb")));

        services.AddScoped<IEkmekBotDbContext>(provider => provider.GetService<EkmekBotDbContext>() ?? throw new InvalidOperationException());

        return services;
    }
}