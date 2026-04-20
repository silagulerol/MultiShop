using MultiShop.DtoLayer.DiscountDtos;

namespace MultiShop.WebUI.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        public HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetDiscountCodeDetailByCode> GetDiscountCode(string code)
        {
            var value= await _httpClient.GetFromJsonAsync<GetDiscountCodeDetailByCode>($"discounts/GetCodeDetailByCode/{code}");
            return value;
        }
    }
}
