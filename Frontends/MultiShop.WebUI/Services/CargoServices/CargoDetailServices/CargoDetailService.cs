using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;

namespace MultiShop.WebUI.Services.CargoServices.CargoDetailServices
{
    public class CargoDetailService : ICargoDetailService
    {
        private readonly HttpClient _httpClient;

        public CargoDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultCargoDetailDto> CreateCargoDetailAsync(CreateCargoDetailDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("cargodetail", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Cargo creation failed: {error}");
            }

            return await response.Content.ReadFromJsonAsync<ResultCargoDetailDto>();
        }

        public async Task<bool> HasCargoAsync(int orderDetailId)
        {
            var response = await _httpClient.GetAsync($"cargodetail/GetByOrderDetailId/{orderDetailId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<ResultCargoDetailDto?> GetByOrderDetailIdAsync(int orderDetailId)
        {
            var response = await _httpClient.GetAsync($"cargodetail/GetByOrderDetailId/{orderDetailId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ResultCargoDetailDto>();
        }
    }
}