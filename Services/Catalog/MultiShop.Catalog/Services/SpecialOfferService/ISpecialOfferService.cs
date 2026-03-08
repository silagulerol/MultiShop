using MultiShop.Catalog.Dtos.SpecialOfferDtos;

namespace MultiShop.Catalog.Services.SpecialOfferService
{
    public interface ISpecialOfferService
    {
        Task<List<ResultSpecialOfferDto>> GetAllSpecialOffersAsync();
        Task<ResultSpecialOfferDto> GetSpecialOfferByIdAsync(string id);
        Task DeleteSpecialOfferAsync(string id);
        Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto);
        Task InsertSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto);
        Task FeatureSliderChangeToFalseAsync(string id);
        Task FeatureSliderChangeToTrueAsync(string id);

    }
}
