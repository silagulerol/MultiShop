using MultiShop.Catalog.Dtos.FeatureSliderDtos;

namespace MultiShop.Catalog.Services.FeatureSliderService
{
    public interface IFeatureSliderService
    {
        Task<List<ResultFeatureSliderDto>> GetAllFeatureSlidersAsync();
        Task<ResultFeatureSliderDto> GetFeatureSliderByIdAsync(string id);
        Task DeleteFeatureSliderAsync(string id);
        Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto);
        Task InsertFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto);
        Task FeatureSliderChangeToTrueAsync(string id);
        Task FeatureSliderChangeToFalseAsync(string id);
    }
}
