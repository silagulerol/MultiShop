using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderOrderingServices
{
    public class OrderOrderingService : IOrderOrderingService
    {
        private readonly HttpClient _httpClient;

        public OrderOrderingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultOrderingByUserId>> GetOrderingByUserId(string id)
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultOrderingByUserId>>($"orderings/GetOrderingsByUserId/{id}");
            return values;
        }
    }
}
