namespace MultiShop.WebUI.Services.StatisticServices.DiscountStatisticServices
{
    public class DiscountStatisticService :IDiscountStatisticService
    {
        private readonly HttpClient _httpClient;
        public DiscountStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetDiscountCouponCount()
        {
            var values = await _httpClient.GetFromJsonAsync<int>("discounts/GetDiscountCouponCount");
            return values;
        }
    }
}
