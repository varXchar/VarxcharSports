using Microsoft.Extensions.Options;
using PuppeteerSharp;
using SportsDataScraperService.Configuration;
using SportsDataScraperService.Controllers;

namespace SportsDataScraperService.Services;

public interface IScoreScraperService
{
    Task GetScoresAsync(string sportName, DateTime? scoresDate);
}

public class ScoreScraperService : IScoreScraperService
{
    private readonly ILogger<ScoreScraperService> _logger;
    private readonly LaunchOptions _launchOptions;
    private readonly ApplicationConfiguration _config;
    private readonly IMessageBrokerService _messageBrokerService;

    public ScoreScraperService(
        ILogger<ScoreScraperService> logger, 
        IOptions<ApplicationConfiguration> config,
        IMessageBrokerService messageBrokerService)
    {
        _logger = logger;
        _config = config.Value;
        _messageBrokerService = messageBrokerService;

        _launchOptions = new LaunchOptions()
        {
            Headless = true, // = false for testing
            ExecutablePath = _config.ChromeExecutablePath
        };
    }

    public async Task GetScoresAsync(string sportName, DateTime? scoresDate)
    {
        scoresDate = scoresDate ?? DateTime.Now;
        await _messageBrokerService.ProduceAsync();

        //using (var browser = await Puppeteer.LaunchAsync(_launchOptions))
        //{
        //    try
        //    {
        //        using (var page = await browser.NewPageAsync())
        //        {
        //            page.Response += GetScoresResponseHandler;

        //            await page.GoToAsync($"{_config.DataScrapingBaseUrl}/{sportName}/events/date/2024-1", WaitUntilNavigation.Networkidle2);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }

    private async void GetScoresResponseHandler(object? sender, ResponseCreatedEventArgs e)
    {
        if (e.Response.Url.Contains(_config.DataScrapingApiUrl))
        {
            var data = await e.Response.TextAsync();

            if (data != null)
            {
                Console.WriteLine("Json Data Received!");
                Console.WriteLine($"{data}");
            }
        }
    }
}
