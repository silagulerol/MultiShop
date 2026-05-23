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
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductDetailDto>>("productdetails");
            return values ?? new List<ResultProductDetailDto>();
        }

        public async Task<UpdateProductDetailDto> GetByIdProductDetailAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UpdateProductDetailDto>($"productdetails/{id}")
                ?? new UpdateProductDetailDto();
        }

        public async Task<UpdateProductDetailDto> GetByProductIdProductDetailAsync(string productId)
        {
            var response = await _httpClient.GetAsync(
                $"productdetails/ProductDetailByProductId/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return new UpdateProductDetailDto
                {
                    ProductId = productId
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new UpdateProductDetailDto
                {
                    ProductId = productId
                };
            }

            return System.Text.Json.JsonSerializer.Deserialize<UpdateProductDetailDto>(
                content,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new UpdateProductDetailDto
                {
                    ProductId = productId
                };
        }

        public async Task<GetByIdProductDetailDto> GetProductDetailByProductIdAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<GetByIdProductDetailDto>($"productdetails/ProductDetailByProductId/{productId}")
                ?? new GetByIdProductDetailDto
                {
                    ProductId = productId,
                    Highlights = new List<string>(),
                    Specifications = new Dictionary<string, string>()
                };
        }

        public async Task<GetByIdProductDetailDto> GetProductDetailByVariantIdAsync(string variantId)
        {
            return await _httpClient.GetFromJsonAsync<GetByIdProductDetailDto>($"productdetails/ProductDetailByVariantId/{variantId}")
                ?? new GetByIdProductDetailDto
                {
                    ProductVariantId = variantId,
                    Highlights = new List<string>(),
                    Specifications = new Dictionary<string, string>()
                };
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            await _httpClient.PutAsJsonAsync("productdetails", updateProductDetailDto);
        }
    }
}
