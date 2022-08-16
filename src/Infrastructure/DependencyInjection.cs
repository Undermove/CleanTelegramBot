using Application.New.Common;
using Infrastructure.New.StateManagement;
using Infrastructure.New.StateManagement.BotCommands;
using Infrastructure.New.StateManagement.BotCommands.TimezoneCommands;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Infrastructure.New;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEkmekBotDbContext>(provider => provider.GetService<EkmekBotDbContext>() ?? throw new InvalidOperationException());
        
        services.AddSingleton<IDialogProcessor, DialogProcessor>();
        services.AddSingleton<IBotCommand, StartCommand>();
        services.AddSingleton<IBotCommand, SettingsCommand>();
        services.AddSingleton<IBotCommand, ChooseTimeZoneCommand>();
        return services;
    }
}