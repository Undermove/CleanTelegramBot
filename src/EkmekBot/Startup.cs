using System.Text.Json.Serialization;
using Application.New;
using EkmekBot.Common;
using EkmekBot.HostedServices;
using Infrastructure.New;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Telegram.Bot;

namespace EkmekBot;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson();

        var botConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        services.AddSingleton(botConfig);
        
        services.AddScoped(_ =>
            new TelegramBotClient(botConfig.Token)
        );
        services.AddApplication();
        services.AddPersistence(Configuration);
        services.AddHealthChecks().AddDbContextCheck<EkmekBotDbContext>();
        services.AddInfrastructure();

        services.AddHostedService<CreateWebhook>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EkmekBot v1"));

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}