using MultiShop.Catalog.Dtos.ProductImageDtos;

namespace MultiShop.Catalog.Services.ProductImageService
{
    public interface IProductImageService
    {
        Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto);
        Task CreateProductImageAsync(CreateProductImageDto createProductImageDto);
        Task DeleteProductImageAsync(string id);
        Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id);
        Task<GetByIdProductImageDto> GetByProductIdProductImageAsync(string id);
        Task<List<ResultProductImageDto>> GetAllProductImageAsync();
    }
}
