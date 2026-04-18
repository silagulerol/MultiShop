using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;

namespace MultiShop.WebUI.Services.CatalogServices.SpecialOfferService
{
    public class SpecialOfferService : ISpecialOfferService
    {
        private readonly HttpClient _httpClient;

        public SpecialOfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task DeleteSpecialOfferAsync(string id)
        {
            await _httpClient.DeleteAsync($"specialOffers?id={id}");
        }

        public Task<List<ResultSpecialOfferDto>> GetAllSpecialOffersAsync()
        {
            return _httpClient.GetFromJsonAsync<List<ResultSpecialOfferDto>>("specialOffers");
        }

        public Task<UpdateSpecialOfferDto> GetSpecialOfferByIdAsync(string id)
        {
            return _httpClient.GetFromJsonAsync<UpdateSpecialOfferDto>($"specialOffers/{id}");
            
        }

        public async Task InsertSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto)
        { 
            await _httpClient.PostAsJsonAsync("specialOffers", createSpecialOfferDto);
        }

        public async Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            await _httpClient.PutAsJsonAsync("specialOffers", updateSpecialOfferDto);
        }
        public Task FeatureSliderChangeToFalseAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task FeatureSliderChangeToTrueAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
