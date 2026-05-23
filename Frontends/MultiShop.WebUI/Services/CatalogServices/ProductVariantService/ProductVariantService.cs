using MultiShop.DtoLayer.CatalogDtos.ProductVariantDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ProductVariantService
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly HttpClient _httpClient;

        public ProductVariantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductVariantAsync(CreateProductVariantDto createProductVariantDto)
        {
            var response = await _httpClient.PostAsJsonAsync("productvariants", createProductVariantDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductVariantAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"productvariants?id={id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ResultProductVariantDto>> GetAllProductVariantAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductVariantDto>>("productvariants")
                ?? new List<ResultProductVariantDto>();
        }

        public async Task<GetByIdProductVariantDto> GetByIdProductVariantAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<GetByIdProductVariantDto>($"productvariants/{id}")
                ?? new GetByIdProductVariantDto();
        }

        public async Task<List<ResultProductVariantDto>> GetProductVariantsByProductIdAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductVariantDto>>($"productvariants/ProductVariantsByProductId/{productId}")
                ?? new List<ResultProductVariantDto>();
        }

        public async Task<List<ResultProductVariantDto>> GetAvailableProductVariantsByProductIdAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ResultProductVariantDto>>($"productvariants/AvailableByProductId/{productId}")
                ?? new List<ResultProductVariantDto>();
        }

        public async Task UpdateProductVariantAsync(UpdateProductVariantDto updateProductVariantDto)
        {
            var response = await _httpClient.PutAsJsonAsync("productvariants", updateProductVariantDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
