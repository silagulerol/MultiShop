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
            var response = await _httpClient.GetAsync($"productdetails/product/{ProductId}");

            if (!response.IsSuccessStatusCode)
            {
                return new UpdateProductDetailDto
                {
                    ProductId = ProductId
                };
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                return new UpdateProductDetailDto
                {
                    ProductId = ProductId
                };
            }

            var value = System.Text.Json.JsonSerializer.Deserialize<UpdateProductDetailDto>(
                json,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return value ?? new UpdateProductDetailDto
            {
                ProductId = ProductId
            };
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            await _httpClient.PutAsJsonAsync("productdetails", updateProductDetailDto);
        }
    }
}
