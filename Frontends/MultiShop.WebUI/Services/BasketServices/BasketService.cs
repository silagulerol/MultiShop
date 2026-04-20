using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            var values = await GetBasketAsync();

            // Sepet hiç yoksa yeni bir tane oluştur
            if (values == null)
            {
                values = new BasketTotalDto();
                values.BasketItems = new List<BasketItemDto>();
            }

            // Ürün sepette var mı bak
            var existingItem = values.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);

            if (existingItem == null)
            {
                // Ürün yoksa listeye ekle
                values.BasketItems.Add(basketItemDto);
            }
            else
            {
                // Ürün varsa miktarını artır (Örnek mantık)
                existingItem.Quantity += basketItemDto.Quantity;
            }

            await SaveBasketAsync(values);
        }

        public async Task DeleteBasketAsync()
        {
            await _httpClient.DeleteAsync("baskets");
        }

        public async Task<BasketTotalDto> GetBasketAsync()
        {
            return await _httpClient.GetFromJsonAsync<BasketTotalDto>("baskets");
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            /*
             * Burada GetBasketAsync() ile sepeti çekiyoruz. Dönen sepet örneği şu şekilde olabilir. 
             * Ardından sepet içerisindeki ürünlerden silmek istediğimiz ürünü bulup, onu listeden kaldırıyoruz.
             * Son olarak güncellenmiş sepeti kaydediyoruz.
             * {
                    "UserId" : "15b6701d-2216-44e9-93d8-bf235a64ffb5",
                    "DiscountCode": "Yok",
                    "DiscountRate" : 0,
                    "BasketItems" : [
                        {
                            "ProductId" : "abc1",
                            "ProductName": "jean",
                            "Quantity": 2,
                            "UnitPrice" : 23
                        },
                        {
                            "ProductId" : "abc2",
                            "ProductName": "table",
                            "Quantity": 2,
                            "UnitPrice" : 23
                        }
                    ]
                }
             */
            var values = await GetBasketAsync();
            var deletedItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            var result = values.BasketItems.Remove(deletedItem);
            await SaveBasketAsync(values);
            return true;
        }

        public async Task SaveBasketAsync(BasketTotalDto basketTotalDto)
        {
            await _httpClient.PostAsJsonAsync($"baskets", basketTotalDto);
        }
    }
}
