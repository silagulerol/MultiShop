using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureSliderService
{
    public interface IFeatureSliderService
    {
        Task<List<ResultFeatureSliderDto>> GetAllFeatureSlidersAsync();
        Task<UpdateFeatureSliderDto> GetFeatureSliderByIdAsync(string id);
        Task DeleteFeatureSliderAsync(string id);
        Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto);
        Task InsertFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto);
        Task FeatureSliderChangeToTrueAsync(string id);
        Task FeatureSliderChangeToFalseAsync(string id);
    }
}
