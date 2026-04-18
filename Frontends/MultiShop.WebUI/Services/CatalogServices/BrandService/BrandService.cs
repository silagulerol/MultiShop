using MultiShop.DtoLayer.CatalogDtos.BrandDtos;

namespace MultiShop.WebUI.Services.CatalogServices.BrandService
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _httpClient;

        public BrandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateBrandAsync(CreateBrandDto createBrandDto)
        {
            await _httpClient.PostAsJsonAsync("brands", createBrandDto);
        }

        public async Task DeleteBrandAsync(string id)
        {
            await _httpClient.DeleteAsync($"brands?id={id}");
        }

        public async Task<List<ResultBrandDto>> GetAllBrandAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultBrandDto>>("brands");
        }

        public async Task<UpdateBrandDto> GetByIdBrandAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateBrandDto>($"brands/{id}");
        }

        public async Task UpdateBrandAsync(UpdateBrandDto updateBrandDto)
        {
            await _httpClient.PutAsJsonAsync("brands", updateBrandDto);
        }
    }
}
