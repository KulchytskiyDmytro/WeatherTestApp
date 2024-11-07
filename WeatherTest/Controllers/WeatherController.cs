using Microsoft.AspNetCore.Mvc;
using WeatherTest.Services;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherService weatherService;

        public WeatherController()
        {
            weatherService = new WeatherService();
        }

        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                city = Request.Cookies["LastCity"];
            }

            if (string.IsNullOrEmpty(city))
            {
                return View();
            }

            try
            {
                var forecast = await weatherService.GetWeatherAsync(city);
                
                Response.Cookies.Append("LastCity", city, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(1) 
                });
                
                string rainWarningCookieKey = "RainWarning_" + city;
                if (forecast.IsRainExpected && Request.Cookies[rainWarningCookieKey] == null)
                {
                    TempData["RainWarning"] = $"Warning: Rain is forecasted in {city} today. Mind having umbrella with you";
                    
                    Response.Cookies.Append(rainWarningCookieKey, "1", new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddDays(1) 
                    });
                }

                return View(forecast);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "City not found or there was an error with the weather service.";
                return View();
            }
        }

    }
}