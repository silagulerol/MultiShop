using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderDetailServices
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly HttpClient _httpClient;

        public OrderDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultOrderDetailDto>> GetOrderDetailsByVendorIdAsync(string vendorId)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultOrderDetailDto>>
                ($"orderdetails/GetByVendorId/{vendorId}");
        }
        public async Task CreateOrderDetailAsync(CreateOrderDetailDto dto)
        {
            await _httpClient.PostAsJsonAsync("orderdetails", dto);
        }
        
        public async Task<List<ResultOrderDetailDto>> GetOrderDetailsByOrderingIdAsync(int orderingId)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultOrderDetailDto>>($"orderdetails/GetByOrderingId/{orderingId}");
        }

        public async Task UpdateOrderDetailAsync(UpdateOrderDetailDto updateOrderDetailDto)
        {
            await _httpClient.PutAsJsonAsync("orderdetails", updateOrderDetailDto);
        }
    }
}