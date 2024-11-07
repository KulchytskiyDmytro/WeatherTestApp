namespace WeatherTest.Models;

public class WeatherForecast
{
    public string City { get; set; }
    public double TemperatureHigh { get; set; }
    public double TemperatureLow { get; set; }
    public double Precipitation { get; set; }
    public bool IsRainExpected { get; set; }
    public DateTime ForecastDate { get; set; }
}