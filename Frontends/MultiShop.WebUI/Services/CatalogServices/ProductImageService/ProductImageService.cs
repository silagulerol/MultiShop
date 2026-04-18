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
            await _httpClient.PostAsJsonAsync("productimages", createProductImageDto);
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
            //var responseMessage = await _httpClient.GetAsync($"productimages/ProductImagesByProductId/{id}");
            //var values= await responseMessage.Content.ReadFromJsonAsync<UpdateProductImageDto>();
            //return values;
            return await _httpClient.GetFromJsonAsync<UpdateProductImageDto>($"productimages/ProductImagesByProductId/{id}");
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            await _httpClient.PutAsJsonAsync("productimages", updateProductImageDto);
        }
    }
}
