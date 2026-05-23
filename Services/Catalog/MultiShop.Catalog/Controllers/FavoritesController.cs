using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.FavoriteDtos;
using MultiShop.Catalog.Services.FavoriteServices;

namespace MultiShop.Catalog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFavorite()
        {
            var values = await _favoriteService.GetAllFavoriteAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdFavorite(string id)
        {
            var value = await _favoriteService.GetByIdFavoriteAsync(id);
            return Ok(value);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFavoritesByUserId(string userId)
        {
            var values = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            return Ok(values);
        }

        [HttpGet("check/{userId}/{productId}")]
        public async Task<IActionResult> IsProductFavorited(string userId, string productId)
        {
            var value = await _favoriteService.IsProductFavoritedAsync(userId, productId);
            return Ok(value);
        }

        [HttpGet("count/{productId}")]
        public async Task<IActionResult> GetFavoriteCountByProductId(string productId)
        {
            var value = await _favoriteService.GetFavoriteCountByProductIdAsync(productId);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavorite(CreateFavoriteDto createFavoriteDto)
        {
            await _favoriteService.CreateFavoriteAsync(createFavoriteDto);
            return Ok();
        }

        [HttpPost("toggle/{userId}/{productId}")]
        public async Task<IActionResult> ToggleFavorite(string userId, string productId)
        {
            await _favoriteService.ToggleFavoriteAsync(userId, productId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFavorite(UpdateFavoriteDto updateFavoriteDto)
        {
            await _favoriteService.UpdateFavoriteAsync(updateFavoriteDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFavorite(string id)
        {
            await _favoriteService.DeleteFavoriteAsync(id);
            return Ok();
        }
    }
}
