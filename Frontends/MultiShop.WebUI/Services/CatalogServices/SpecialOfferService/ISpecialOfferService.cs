using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;

namespace MultiShop.WebUI.Services.CatalogServices.SpecialOfferService
{
    public interface ISpecialOfferService
    {
        Task<List<ResultSpecialOfferDto>> GetAllSpecialOffersAsync();
        Task<UpdateSpecialOfferDto> GetSpecialOfferByIdAsync(string id);
        Task DeleteSpecialOfferAsync(string id);
        Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto);
        Task InsertSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto);
        Task FeatureSliderChangeToFalseAsync(string id);
        Task FeatureSliderChangeToTrueAsync(string id);
    }
}
