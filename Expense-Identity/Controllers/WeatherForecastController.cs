using System.Security.Claims;
using Expense_Identity.Constants;
using Expense_Identity.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    CurrentUser _currentUser;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IHttpContextAccessor httpContextAccessor,
            CurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Authorize(Roles = UserRoles.ADMIN)]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var user = await _currentUser.GetCurrentUser();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            User = user
        })
        .ToArray();
    }
}

