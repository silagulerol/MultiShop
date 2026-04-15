using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using System.Net.Http;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);

        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync($"products?id={id}");
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("products");
        }

        public async Task<UpdateProductDto> GetByIdProductAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateProductDto>($"products/{id}");
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }
    }
}
