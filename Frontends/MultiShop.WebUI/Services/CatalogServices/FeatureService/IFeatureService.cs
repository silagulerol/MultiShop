using MultiShop.DtoLayer.CatalogDtos.FeatureDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureService
{
    public interface IFeatureService
    {
        Task<List<ResultFeatureDto>> GetAllFeatureAsync();
        Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto);
        Task CreateFeatureAsync(CreateFeatureDto createFeatureDto);
        Task DeleteFeatureAsync(string id);
        Task<UpdateFeatureDto> GetByIdFeatureAsync(string id);
    }
}
