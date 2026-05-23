using MultiShop.DtoLayer.CatalogDtos.FavoriteDtos;

namespace MultiShop.WebUI.Services.CatalogServices.FavoriteServices
{
    public interface IFavoriteService
    {
        Task<List<ResultFavoriteDto>> GetFavoritesByUserIdAsync(string userId);
        Task ToggleFavoriteAsync(string userId, string productId);
        Task<bool> IsProductFavoritedAsync(string userId, string productId);
        Task<int> GetFavoriteCountByProductIdAsync(string productId);
    }
}
