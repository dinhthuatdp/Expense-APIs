using _2.ExpenseManagement.Api.Entities;

namespace _2.ExpenseManagement.Api;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    public IEnumerable<Category>? MyProperty { get; set; }
}

