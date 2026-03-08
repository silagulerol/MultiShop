using MultiShop.Catalog.Dtos.FeatureDtos;

namespace MultiShop.Catalog.Services.FeatureService
{
    public interface IFeatureService
    {
        Task<List<ResultFeatureDto>> GetAllFeatureAsync();
        Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto);
        Task CreateFeatureAsync(CreateFeatureDto createFeatureDto);
        Task DeleteFeatureAsync(string id);
        Task<ResultFeatureDto> GetByIdFeatureAsync(string id);
    }
}
