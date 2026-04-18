using MultiShop.DtoLayer.CatalogDtos.ProductDetailDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductDetailService
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly HttpClient _httpClient;

        public ProductDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
        {
            await _httpClient.PostAsJsonAsync("productdetails", createProductDetailDto);
        }

        public async Task DeleteProductDetailAsync(string id)
        {
            await _httpClient.DeleteAsync($"productdetails?id={id}");
        }

        public async Task<List<ResultProductDetailDto>> GetAllProductDetailAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductDetailDto>>("productdetails");
        }

        public async Task<UpdateProductDetailDto> GetByIdProductDetailAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateProductDetailDto>($"productdetails/{id}");
        }

        public async Task<UpdateProductDetailDto> GetByProductIdProductDetailAsync(string ProductId)
        {
            return await _httpClient.GetFromJsonAsync<UpdateProductDetailDto>($"productdetails/product/{ProductId}");
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            await _httpClient.PutAsJsonAsync("productdetails", updateProductDetailDto);
        }
    }
}
