using Microsoft.AspNetCore.Mvc;
using MultiShop.RapidApi.Models;
using Newtonsoft.Json;

namespace MultiShop.RapidApi.Controllers
{
    public class ECommerceController : Controller
    {
        public async Task<IActionResult> ECommerceList(string q = "nike shoe", string country = "us")
        {
            var client = new HttpClient();
            var encodedQuery = Uri.EscapeDataString(q);
            var encodedCountry = Uri.EscapeDataString(country);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://real-time-product-search.p.rapidapi.com/search-v2?q={encodedQuery}&country={encodedCountry}&language=en&page=1&limit=10&sort_by=BEST_MATCH&product_condition=ANY&return_filters=true"),
                Headers =
                    {
                        { "x-rapidapi-key", "8852ff42edmsh703a0b1a830bfb0p1527b5jsn663fcbbe9a38" },
                        { "x-rapidapi-host", "real-time-product-search.p.rapidapi.com" },
                    },
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ECommerceViewModel.Rootobject>(body);

            var products = result?.data?.products?
                .Where(x => !string.IsNullOrWhiteSpace(x.product_title))
                .ToList();

            ViewBag.Query = q;
            ViewBag.Country = country;

            return View(products);
        }
    }
}
