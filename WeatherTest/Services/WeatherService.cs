using Newtonsoft.Json;
using WeatherTest.Models;

namespace WeatherTest.Services;

public class WeatherService
{
    private readonly string apiKey = "64cc4aa081d61d9fb5e5c72a2192be9b";
    private readonly HttpClient httpClient;

    public WeatherService()
    {
        httpClient = new HttpClient();
    }

    public async Task<WeatherForecast> GetWeatherAsync(string city)
    {
        var response = await httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric");

        if (response.IsSuccessStatusCode)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            var forecast = new WeatherForecast
            {
                City = data.name,
                TemperatureHigh = data.main.temp_max,
                TemperatureLow = data.main.temp_min,
                Precipitation = data.rain?["1h"] ?? 0,
                IsRainExpected = data.weather[0].main == "Rain",
                ForecastDate = DateTime.Now
            };

            return forecast;
        }

        throw new Exception("City not found");
    }
}