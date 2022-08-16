using System.Text.Json.Serialization;
using Application.New;
using EkmekBot.Common;
using EkmekBot.HostedServices;
using EkmekBot.Jobs;
using Infrastructure.New;
using Infrastructure.New.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notifications;
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

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options=>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson();

        services.AddSwaggerGen(c =>  
        {  
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuth", Version = "v1" });  
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme  
            {  
                Name = "Authorization",  
                Type = SecuritySchemeType.Http,  
                Scheme = "basic",  
                In = ParameterLocation.Header,  
                Description = "Basic Authorization header using the Bearer scheme."  
            });  
            c.AddSecurityRequirement(new OpenApiSecurityRequirement  
            {  
                {  
                    new OpenApiSecurityScheme  
                    {  
                        Reference = new OpenApiReference  
                        {  
                            Type = ReferenceType.SecurityScheme,  
                            Id = "basic"  
                        }  
                    },  
                    new string[] {}  
                }  
            });  
        });  

        var botConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        services.AddSingleton(botConfig);

        var authConfig = Configuration.GetSection("AuthConfiguration").Get<AuthConfiguration>();
        services.AddSingleton(authConfig);
        services.AddAuthentication("BasicAuthentication")  
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);  
  
        services.AddScoped<IUserService, UserService>();
        services.AddHostedService<ReminderJob>();
        services.AddScoped(_ => 
            new TelegramBotClient(botConfig.Token)
        );
        services.AddApplication();
        services.AddPersistence(Configuration);
        services.AddHealthChecks().AddDbContextCheck<EkmekBotDbContext>();
        services.AddInfrastructure();
        services.AddNotifications(Configuration);
        
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

        app.UseAuthentication();  
        app.UseAuthorization();
        
        app.UseNotifications();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}