using MultiShop.DtoLayer.CatalogDtos.FavoriteDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FavoriteServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly HttpClient _httpClient;

        public FavoriteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultFavoriteDto>> GetFavoritesByUserIdAsync(string userId)
        {
            var values = await _httpClient.GetFromJsonAsync<List<ResultFavoriteDto>>($"favorites/user/{userId}");
            return values ?? new List<ResultFavoriteDto>();
        }

        public async Task ToggleFavoriteAsync(string userId, string productId)
        {
            var response = await _httpClient.PostAsync($"favorites/toggle/{userId}/{productId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsProductFavoritedAsync(string userId, string productId)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"favorites/check/{userId}/{productId}");
        }

        public async Task<int> GetFavoriteCountByProductIdAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<int>($"favorites/count/{productId}");
        }
    }
}
