using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SportsDataScraperService.Services;

namespace SportsDataScraperService.Controllers;

[ApiController]
[Route("[controller]")]
public class ScoresController(ILogger<ScoresController> logger, IScoreScraperService scoreScraperService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        await scoreScraperService.GetScoresAsync("mlb", DateTime.Now);
        return Ok();
    }

    //[HttpGet]
    //public IEnumerable<WeatherForecast> GetAll()
    //{
    //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //    {
    //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //        TemperatureC = Random.Shared.Next(-20, 55),
    //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //    })
    //    .ToArray();
    //}
}
