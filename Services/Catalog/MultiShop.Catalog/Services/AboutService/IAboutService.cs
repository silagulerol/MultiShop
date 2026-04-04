using MultiShop.Catalog.Dtos.AboutDtos;

namespace MultiShop.Catalog.Services.AboutService
{
    public interface IAboutService
    {
        Task<List<ResultAboutDto>> GetAllAboutAsync();
        Task UpdateAboutAsync(UpdateAboutDto updateAboutDto);

        Task CreateAboutAsync(CreateAboutDto createAboutDto);
        Task DeleteAboutAsync(string id);
        Task<ResultAboutDto> GetByIdAboutAsync(string id);
    }
}
