using MultiShop.DtoLayer.CargoDtos.CargoOperationDtos;

namespace MultiShop.WebUI.Services.CargoServices.CargoOperationServices
{
    public class CargoOperationService : ICargoOperationService
    {
        private readonly HttpClient _httpClient;

        public CargoOperationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCargoOperationAsync(CreateCargoOperationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("cargooperation", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Cargo operation creation failed: {error}");
            }
        }
    }
}