namespace MultiShop.WebUI.Services.StatisticServices.CatalogStatisticServices
{
    public class CatalogStatisticService : ICatalogStatisticService
    {
        private readonly HttpClient _httpClient;
        public CatalogStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<long> GetBrandCount()
        {
            var values= await _httpClient.GetFromJsonAsync<long>("Statistics/GetBrandCount");
            return values;
        }

        public async Task<long> GetCategoryCount()
        {
            var values = await _httpClient.GetFromJsonAsync<long>("Statistics/GetCategoryCount");
            return values;
        }


        public async Task<string> GetMaxPriceProductName()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetMaxPriceProductName");
            var values = await responseMessage.Content.ReadAsStringAsync();
            return values;
        }

        public async Task<string> GetMinPriceProductName()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetMinPriceProductName");
            var values = await responseMessage.Content.ReadAsStringAsync();
            return values;
        }

        //public async Task<decimal> GetProductAvgPrice()
        //{
        //    var values = await _httpClient.GetFromJsonAsync<decimal>("Statistics/GetProductAvgPrice");
        //    return values;
        //}

        public async Task<long> GetProductCount()
        {
            var values = await _httpClient.GetFromJsonAsync<long>("Statistics/GetProductCount");
            return values;
        }
    }
}
