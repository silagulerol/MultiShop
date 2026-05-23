using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public class OrderAddressService : IOrderAddressService
    {
        public HttpClient _httpClient;

        public OrderAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateAddressAsync(CreateAddressDto createAddressDto)
        {
            var response = await _httpClient.PostAsJsonAsync("addresses", createAddressDto);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ResultAddressDto>();
            return result?.AddressId ?? 0;
        }

        public async Task<ResultAddressDto?> GetAddressByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            var response = await _httpClient.GetAsync($"addresses/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<ResultAddressDto>(
                content,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<List<ResultAddressDto>> GetAddressesByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<ResultAddressDto>();
            }

            var result = await _httpClient.GetFromJsonAsync<List<ResultAddressDto>>($"addresses/GetByUserId/{userId}");
            return result ?? new List<ResultAddressDto>();
        }
    }
}
