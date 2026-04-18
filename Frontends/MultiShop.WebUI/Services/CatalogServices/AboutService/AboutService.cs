using MultiShop.DtoLayer.CatalogDtos.AboutDtos;

namespace MultiShop.WebUI.Services.CatalogServices.AboutService
{
    public class AboutService : IAboutService
    {
        private readonly HttpClient _httpClient;

        public AboutService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateAboutAsync(CreateAboutDto createAboutDto)
        {
            await _httpClient.PostAsJsonAsync("abouts", createAboutDto);
        }

        public async Task DeleteAboutAsync(string id)
        {
            await _httpClient.DeleteAsync($"abouts?id={id}");
        }

        public async Task<List<ResultAboutDto>> GetAllAboutAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultAboutDto>>("abouts");
        }

        public async Task<UpdateAboutDto> GetByIdAboutAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateAboutDto>($"abouts/{id}");
        }

        public Task UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            return _httpClient.PutAsJsonAsync("abouts", updateAboutDto);
        }
    }
}
