using MultiShop.Catalog.Dtos.ProductImageDtos;

namespace MultiShop.Catalog.Services.ProductImageService
{
    public interface IProductImageService
    {
        Task<List<ResultProductImageDto>> GetAllProductImageAsync();
        Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto);
        Task CreateProductImageAsync(CreateProductImageDto createProductImageDto);
        Task DeleteProductImageAsync(string id);
        Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id);
        Task<List<ResultProductImageDto>> GetImagesByProductIdAsync(string productId);
        Task<List<ResultProductImageDto>> GetImagesByVariantIdAsync(string productVariantId);
        Task<GetByIdProductImageDto> GetMainImageByProductIdAsync(string productId);
        Task<GetByIdProductImageDto> GetMainImageByVariantIdAsync(string productVariantId);
    }
}
