using MultiShop.Catalog.Dtos.CategoryDtos;
using MultiShop.Catalog.Dtos.ProductDetailDtos;

namespace MultiShop.Catalog.Services.ProductDetailService
{
    public interface IProductDetailService
    {
        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto);
        Task CreateProductDetailAsync( CreateProductDetailDto createProductDetailDto);
        Task DeleteProductDetailAsync(string id);
        Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id);
        Task<List<ResultProductDetailDto>> GetAllProductDetailAsync();
    }
}
