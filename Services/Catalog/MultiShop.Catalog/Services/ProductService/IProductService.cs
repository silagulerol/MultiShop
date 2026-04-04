
using MultiShop.Catalog.Dtos.ProductDtos;

namespace MultiShop.Catalog.Services.ProductService
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();


        //Bu method async çalışır ama geriye değer döndürmez.
        //Yani async versiyonu void gibi düşünebilirsin. return olmaz 
        Task UpdateProductAsync(UpdateProductDto updateProductDto);

        Task CreateProductAsync(CreateProductDto createProductDto);
        Task DeleteProductAsync(string id);
        Task<GetByIdProductDto> GetByIdProductAsync(string id);
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync();
        Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId);
    }
}
