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

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("products");
        }
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);

        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync($"products?id={id}");
        }


        public async Task<UpdateProductDto> GetByIdProductAsync(string id)
        {
            //var responseMessage = await _httpClient.GetAsync($"products/{id}");
            //var values = await responseMessage.Content.ReadFromJsonAsync<UpdateProductDto>();
            //return values;
            return await _httpClient.GetFromJsonAsync<UpdateProductDto>($"products/{id}");
        }

       

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>("products/ProductListWithCategory");
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>($"products/ProductListWithCategoryByCategoryId/{CategoryId}");
        }

      
    }
}
