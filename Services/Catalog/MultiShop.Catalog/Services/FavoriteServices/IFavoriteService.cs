using MultiShop.Catalog.Dtos.FavoriteDtos;

namespace MultiShop.Catalog.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        Task<List<ResultFavoriteDto>> GetAllFavoriteAsync();
        Task CreateFavoriteAsync(CreateFavoriteDto createFavoriteDto);
        Task UpdateFavoriteAsync(UpdateFavoriteDto updateFavoriteDto);
        Task DeleteFavoriteAsync(string id);
        Task<GetByIdFavoriteDto> GetByIdFavoriteAsync(string id);
        Task<List<ResultFavoriteDto>> GetFavoritesByUserIdAsync(string userId);
        Task<bool> IsProductFavoritedAsync(string userId, string productId);
        Task ToggleFavoriteAsync(string userId, string productId);
        Task<int> GetFavoriteCountByProductIdAsync(string productId);
    }
}
