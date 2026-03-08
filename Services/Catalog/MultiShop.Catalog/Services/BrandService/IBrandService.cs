using MultiShop.Catalog.Dtos.BrandDtos;
using System.Threading.Tasks;

namespace MultiShop.Catalog.Services.BrandService
{
    public interface IBrandService
    {
        Task<List<ResultBrandDto>> GetAllBrandAsync();

        Task UpdateBrandAsync(UpdateBrandDto updateBrandDto);

        Task CreateBrandAsync(CreateBrandDto createBrandDto);
        Task DeleteBrandAsync(string id);
        Task<ResultBrandDto> GetByIdBrandAsync(string id);
    }
}
