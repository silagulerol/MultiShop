using System.Text.Json;
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
            var response = await _httpClient.PostAsJsonAsync(
                "productimages",
                createProductImageDto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"ProductImage create failed. Status: {response.StatusCode}, Error: {error}");
            }
        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _httpClient.DeleteAsync($"productimages?id={id}");
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductImageDto>>(
                "productimages");

            return values ?? new List<ResultProductImageDto>();
        }

        public async Task<UpdateProductImageDto> GetByIdProductImageAsync(string id)
        {
            var response = await _httpClient.GetAsync($"productimages/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return new UpdateProductImageDto();
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new UpdateProductImageDto();
            }

            return JsonSerializer.Deserialize<UpdateProductImageDto>(
                       content,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       })
                   ?? new UpdateProductImageDto();
        }

        public async Task<UpdateProductImageDto> GetByProductIdProductImageAsync(string id)
        {
            var response = await _httpClient.GetAsync(
                $"productimages/MainImageByProductId/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return new UpdateProductImageDto
                {
                    ProductId = id,
                    CreatedDate = DateTime.UtcNow,
                    ImageType = "Main",
                    DisplayOrder = 0,
                    IsMainImage = true
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new UpdateProductImageDto
                {
                    ProductId = id,
                    CreatedDate = DateTime.UtcNow,
                    ImageType = "Main",
                    DisplayOrder = 0,
                    IsMainImage = true
                };
            }

            return JsonSerializer.Deserialize<UpdateProductImageDto>(
                       content,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       })
                   ?? new UpdateProductImageDto
                   {
                       ProductId = id,
                       CreatedDate = DateTime.UtcNow,
                       ImageType = "Main",
                       DisplayOrder = 0,
                       IsMainImage = true
                   };
        }

        public async Task<List<ResultProductImageDto>> GetImagesByProductIdAsync(
            string productId)
        {
            var values =
                await _httpClient.GetFromJsonAsync<List<ResultProductImageDto>>(
                    $"productimages/ImagesByProductId/{productId}");

            return values ?? new List<ResultProductImageDto>();
        }

        public async Task<List<ResultProductImageDto>> GetImagesByVariantIdAsync(
            string variantId)
        {
            var values =
                await _httpClient.GetFromJsonAsync<List<ResultProductImageDto>>(
                    $"productimages/ImagesByVariantId/{variantId}");

            return values ?? new List<ResultProductImageDto>();
        }

        public async Task<ResultProductImageDto> GetMainImageByProductIdAsync(
            string productId)
        {
            var response = await _httpClient.GetAsync(
                $"productimages/MainImageByProductId/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return new ResultProductImageDto
                {
                    ProductId = productId,
                    CreatedDate = DateTime.UtcNow
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new ResultProductImageDto
                {
                    ProductId = productId,
                    CreatedDate = DateTime.UtcNow
                };
            }

            return JsonSerializer.Deserialize<ResultProductImageDto>(
                       content,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       })
                   ?? new ResultProductImageDto
                   {
                       ProductId = productId,
                       CreatedDate = DateTime.UtcNow
                   };
        }

        public async Task<ResultProductImageDto> GetMainImageByVariantIdAsync(
            string variantId)
        {
            var response = await _httpClient.GetAsync(
                $"productimages/MainImageByVariantId/{variantId}");

            if (!response.IsSuccessStatusCode)
            {
                return new ResultProductImageDto
                {
                    ProductVariantId = variantId,
                    CreatedDate = DateTime.UtcNow
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new ResultProductImageDto
                {
                    ProductVariantId = variantId,
                    CreatedDate = DateTime.UtcNow
                };
            }

            return JsonSerializer.Deserialize<ResultProductImageDto>(
                       content,
                       new JsonSerializerOptions
                       {
                           PropertyNameCaseInsensitive = true
                       })
                   ?? new ResultProductImageDto
                   {
                       ProductVariantId = variantId,
                       CreatedDate = DateTime.UtcNow
                   };
        }

        public async Task UpdateProductImageAsync(
            UpdateProductImageDto updateProductImageDto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                "productimages",
                updateProductImageDto);

            response.EnsureSuccessStatusCode();
        }
    }
}