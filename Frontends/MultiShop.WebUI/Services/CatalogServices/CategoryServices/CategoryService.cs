using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;

namespace MultiShop.WebUI.Services.CatalogServices.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            // program.cs de AddHttpClient ile eklediğimiz için base adresi otomatik olarak eklenecektir. http://localhost:5000/services/catalog/ base address
            // Burada categories ekleyerek tam url'i oluşturuyoruz. http://localhost:5000/services/catalog/categories
            // Aynı zamanda AddHttpMessageHandler<ClientCredentialTokenHandler>(); eklediğimiz için
            // ClientCredentialTokenHandler ve token'ı alarak istek atarken header'a ekleyecektir.
            var response = await _httpClient.PostAsJsonAsync<CreateCategoryDto>("categories", createCategoryDto);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _httpClient.DeleteAsync($"categories?id={id}");
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultCategoryDto>>("categories");
        }

        public Task<UpdateCategoryDto> GetByIdCategoryAsync(string id)
        {
            return _httpClient.GetFromJsonAsync<UpdateCategoryDto>($"categories/{id}");
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCategoryDto>("categories", updateCategoryDto);
        }
    }
}
