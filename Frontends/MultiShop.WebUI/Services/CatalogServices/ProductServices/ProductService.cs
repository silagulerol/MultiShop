using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using System.Globalization;

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
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("products");
            return values ?? new List<ResultProductDto>();
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(string id)
        {
            await _httpClient.DeleteAsync($"products?id={id}");
        }


        public async Task<UpdateProductDto> GetByIdProductAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new UpdateProductDto();
            }

            var response = await _httpClient.GetAsync($"products/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return new UpdateProductDto();
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new UpdateProductDto();
            }

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<UpdateProductDto>(
                    content,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new UpdateProductDto();
            }
            catch (System.Text.Json.JsonException)
            {
                return new UpdateProductDto();
            }
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>("products/ProductListWithCategory");
            return values ?? new List<ResultProductWithCategoryDto>();
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>($"products/ProductListWithCategoryByCategoryId/{CategoryId}");
            return values ?? new List<ResultProductWithCategoryDto>();
        }

        public async Task<List<ResultProductDto>> GetProductsByVendorIdAsync(string vendorId)
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductDto>>(
                $"products/GetProductsByVendorId/{vendorId}");

            return values ?? new List<ResultProductDto>();
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByVendorIdAsync(string vendorId)
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>(
                $"products/ProductListWithCategoryByVendorId/{vendorId}");

            return values ?? new List<ResultProductWithCategoryDto>();
        }

        public async Task<List<ResultProductDto>> SearchProductAsync(string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey))
                return new List<ResultProductDto>();

            var responseMessage = await _httpClient.GetAsync(
                $"products/SearchProduct?searchKey={Uri.EscapeDataString(searchKey)}"
            );

            if (!responseMessage.IsSuccessStatusCode)
                return new List<ResultProductDto>();

            var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultProductDto>>();
            return values ?? new List<ResultProductDto>();
        }

        public async Task<ProductFilterResponseDto> FilterProductsAsync(ProductFilterRequestDto request)
        {
            var queryString = BuildFilterQueryString(request);
            var responseMessage = await _httpClient.GetAsync($"products/FilterProducts{queryString}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new ProductFilterResponseDto
                {
                    Page = request.Page,
                    PageSize = request.PageSize
                };
            }

            var values = await responseMessage.Content.ReadFromJsonAsync<ProductFilterResponseDto>();
            return values ?? new ProductFilterResponseDto
            {
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        private static string BuildFilterQueryString(ProductFilterRequestDto request)
        {
            var query = new List<string>();

            AddValue(query, "CategoryId", request.CategoryId);
            AddValues(query, "Brands", request.Brands);
            AddDecimal(query, "MinPrice", request.MinPrice);
            AddDecimal(query, "MaxPrice", request.MaxPrice);
            AddValues(query, "Colors", request.Colors);
            AddValues(query, "Sizes", request.Sizes);
            AddValues(query, "Materials", request.Materials);
            AddValues(query, "Capacities", request.Capacities);
            AddValues(query, "Styles", request.Styles);
            AddDouble(query, "Rating", request.Rating);
            AddValue(query, "SortBy", request.SortBy);
            AddInt(query, "Page", request.Page);
            AddInt(query, "PageSize", request.PageSize);
            AddBool(query, "IsFreeShipping", request.IsFreeShipping);
            AddBool(query, "IsBestSeller", request.IsBestSeller);
            AddBool(query, "IsFeatured", request.IsFeatured);
            AddBool(query, "IsRecyclable", request.IsRecyclable);
            AddBool(query, "IsWomenEntrepreneur", request.IsWomenEntrepreneur);
            AddBool(query, "InStock", request.InStock);

            return query.Any() ? $"?{string.Join("&", query)}" : string.Empty;
        }

        private static void AddValues(List<string> query, string key, IEnumerable<string>? values)
        {
            if (values is null)
            {
                return;
            }

            foreach (var value in values.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                AddValue(query, key, value);
            }
        }

        private static void AddValue(List<string> query, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                query.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
            }
        }

        private static void AddDecimal(List<string> query, string key, decimal? value)
        {
            if (value.HasValue)
            {
                AddValue(query, key, value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static void AddDouble(List<string> query, string key, double? value)
        {
            if (value.HasValue)
            {
                AddValue(query, key, value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static void AddInt(List<string> query, string key, int value)
        {
            if (value > 0)
            {
                AddValue(query, key, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static void AddBool(List<string> query, string key, bool? value)
        {
            if (value == true)
            {
                AddValue(query, key, "true");
            }
        }
    }
}
