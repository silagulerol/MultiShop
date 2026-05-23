using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductImageService
{
    public interface IProductImageService
    {
        Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto);
        Task CreateProductImageAsync(CreateProductImageDto createProductImageDto);
        Task DeleteProductImageAsync(string id);
        Task<UpdateProductImageDto> GetByIdProductImageAsync(string id);
        Task<UpdateProductImageDto> GetByProductIdProductImageAsync(string id);
        Task<List<ResultProductImageDto>> GetImagesByProductIdAsync(string productId);
        Task<List<ResultProductImageDto>> GetImagesByVariantIdAsync(string variantId);
        Task<ResultProductImageDto> GetMainImageByProductIdAsync(string productId);
        Task<ResultProductImageDto> GetMainImageByVariantIdAsync(string variantId);
        Task<List<ResultProductImageDto>> GetAllProductImageAsync();

    }
}
