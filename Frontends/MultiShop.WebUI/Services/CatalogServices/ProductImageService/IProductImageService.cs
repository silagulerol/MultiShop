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
        Task<List<ResultProductImageDto>> GetAllProductImageAsync();

    }
}
