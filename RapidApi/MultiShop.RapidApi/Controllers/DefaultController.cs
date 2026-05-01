using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MultiShop.RapidApi.Controllers
{
    public class DefaultController : Controller
    {
        public async Task<IActionResult> WeatherDetail()
        {
            var city = "Izmir"; // kullanıcıdan alınabilir

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://open-weather13.p.rapidapi.com/city?city={city}&units=metric"),
                Headers =
                    {
                        { "x-rapidapi-key", "8852ff42edmsh703a0b1a830bfb0p1527b5jsn663fcbbe9a38" },
                        { "x-rapidapi-host", "open-weather13.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(body);
                var temp = json.RootElement
                    .GetProperty("main")
                    .GetProperty("temp")
                    .GetDouble();

                var tempCelsius = (temp - 32) * 5 / 9;
                Console.WriteLine($"{city} sıcaklık: {tempCelsius:F1} °C");
                ViewBag.Temp = tempCelsius.ToString("F1");

            }
            return View();
        }

        [HttpGet]
        //public async Task<IActionResult> Exchange()
        //{
        //    return View();
        //}

        //[HttpPost]
        public async Task<IActionResult> Exchange(string from_symbol = "USD", string to_symbol = "TRY")
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://real-time-finance-data.p.rapidapi.com/currency-exchange-rate?from_symbol={from_symbol}&to_symbol={to_symbol}&language=en"),
                Headers =
                    {
                        { "x-rapidapi-key", "8852ff42edmsh703a0b1a830bfb0p1527b5jsn663fcbbe9a38" },
                        { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(body);
                var exchange_rate = json.RootElement
                    .GetProperty("data")
                    .GetProperty("exchange_rate")
                    .GetDouble();

                Console.WriteLine($"{from_symbol} to {to_symbol} exchange rate: {exchange_rate}");
                
                ViewBag.FromSymbol = from_symbol;
                ViewBag.ToSymbol = to_symbol;
                ViewBag.ExchangeRate = exchange_rate.ToString("F2");
            }


            return View();
        }
    }
}
