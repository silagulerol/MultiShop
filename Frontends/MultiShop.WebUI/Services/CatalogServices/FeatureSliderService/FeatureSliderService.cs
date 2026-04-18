using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureSliderService
{
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly HttpClient _httpClient;

        public FeatureSliderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteFeatureSliderAsync(string id)
        {
            await _httpClient.DeleteAsync($"featureSliders?id={id}");
        }

        public Task FeatureSliderChangeToFalseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task FeatureSliderChangeToTrueAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSlidersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultFeatureSliderDto>>("featureSliders");
        }

        public async Task<UpdateFeatureSliderDto> GetFeatureSliderByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateFeatureSliderDto>($"featureSliders/{id}");
        }

        public async Task InsertFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto)
        {
            await  _httpClient.PostAsJsonAsync("featureSliders", createFeatureSliderDto);
        }

        public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            await _httpClient.PutAsJsonAsync("featureSliders", updateFeatureSliderDto);
        }
    }
}
