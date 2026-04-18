using MultiShop.DtoLayer.CatalogDtos.ProductDetailDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductDetailService
{
    public interface IProductDetailService
    {
        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto);
        Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto);
        Task DeleteProductDetailAsync(string id);
        Task<UpdateProductDetailDto> GetByIdProductDetailAsync(string id);
        Task<List<ResultProductDetailDto>> GetAllProductDetailAsync();
        Task<UpdateProductDetailDto> GetByProductIdProductDetailAsync(string ProductId);
    }
}
