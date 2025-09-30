using Microsoft.AspNetCore.Mvc;

namespace geo_restriction_midleware.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class GetWeatherForecastController:ControllerBase
{
    private readonly string[] _summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private List<WeatherForecast> _forecasts = new();

    [HttpGet]
    public IActionResult GetWeather()
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
        {
            return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            };
        }).ToArray();

        return Ok(forecast);
    }

    [HttpGet("forecast")]
    public IActionResult GetForcast([FromQuery] int days = 5)
    {
        var forecast = Enumerable.Range(1, days).Select(index =>
        {
            return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            };
        }).ToArray();

        return Ok(forecast);

    }

    [HttpGet("{days}")]
    public IActionResult GetDayForecast(int days = 1)
    {
        var forecast = Enumerable.Range(1, days).Select(index =>
        {
            return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            };
        }).ToArray();

        return Ok(forecast);
        
    }

    [HttpPost]
    public IActionResult CreateWeather([FromBody] WeatherForecast weather)
    {
        _forecasts.Add(weather);
        return Ok(string.Join(", ", _forecasts));

    }
}