using System.Net.Http.Json;
namespace MultiShop.WebUI.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CompletePaymentAsync()
        {
            var response = await _httpClient.PostAsync("payment", null);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Payment failed: {response.StatusCode} - {error}");
            }
        }
    }
}