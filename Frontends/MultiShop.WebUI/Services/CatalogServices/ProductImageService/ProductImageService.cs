using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductImageService
{
    public class ProductImageService : IProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            var response = await _httpClient.PostAsJsonAsync("productimages", createProductImageDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ProductImage create failed. Status: {response.StatusCode}, Error: {error}");
            }
        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _httpClient.DeleteAsync($"productimages?id={id}");
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductImageDto>>("productimages");
        }

        public async Task<UpdateProductImageDto> GetByIdProductImageAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateProductImageDto>($"productimages/{id}");
        }

        public async Task<UpdateProductImageDto> GetByProductIdProductImageAsync(string id)
        {
            var response = await _httpClient.GetAsync($"productimages/ProductImagesByProductId/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return new UpdateProductImageDto
                {
                    ProductId = id
                };
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                return new UpdateProductImageDto
                {
                    ProductId = id
                };
            }

            var value = System.Text.Json.JsonSerializer.Deserialize<UpdateProductImageDto>(
                json,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return value ?? new UpdateProductImageDto
            {
                ProductId = id
            };
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            var response= await _httpClient.PutAsJsonAsync("productimages", updateProductImageDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
