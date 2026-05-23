using MultiShop.Basket.Dtos;
using MultiShop.Basket.Settings;
using System.Text.Json;

namespace MultiShop.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        // GetDb() methodu ile Database'i getiririz.
        // Ardından db işlemleri için metodalrı çağırırız: StringGetAsync, 
        public async Task DeleteBasketAsync(string userId)
        {
            await _redisService.GetDb().KeyDeleteAsync(userId);
        }

        public async Task<BasketTotalDto> GetBasketAsync(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
            {
                return new BasketTotalDto
                {
                    UserId = userId,
                    BasketItems = new List<BasketItemDto>(),
                    DiscountCode = "Yok",
                    DiscountRate = 0,
                    ShippingPrice = 0
                };
            }

            return JsonSerializer.Deserialize<BasketTotalDto>(existBasket);
        }

        public async Task SaveBasketAsync(BasketTotalDto basketTotalDto)
        {
            //Kayderderken bir key bir de value değeri atamam lazım. Key olarak UserId, value olarak basketin kendisini atadım.
            await _redisService.GetDb().StringSetAsync(basketTotalDto.UserId, JsonSerializer.Serialize(basketTotalDto));
        }
    }
}
