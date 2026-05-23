using MultiShop.Catalog.Dtos.ProductVariantDtos;

namespace MultiShop.Catalog.Services.ProductVariantService
{
    public interface IProductVariantService
    {
        Task<List<ResultProductVariantDto>> GetAllProductVariantAsync();
        Task<List<ResultProductVariantDto>> GetProductVariantsByProductIdAsync(string productId);
        Task<List<ResultProductVariantDto>> GetAvailableProductVariantsByProductIdAsync(string productId);
        Task<GetByIdProductVariantDto> GetByIdProductVariantAsync(string id);
        Task CreateProductVariantAsync(CreateProductVariantDto createProductVariantDto);
        Task UpdateProductVariantAsync(UpdateProductVariantDto updateProductVariantDto);
        Task DeleteProductVariantAsync(string id);
    }
}
