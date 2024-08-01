using SportsDataScraperService.Configuration;
using SportsDataScraperService.Services;

namespace SportsDataScraperService;

public static class ConfigureServices
{
    public static void AddApplicationConfiguration(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddOptions<ApplicationConfiguration>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static void AddServices(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddSingleton<IScoreScraperService, ScoreScraperService>();
        builder.Services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {

    }
}
