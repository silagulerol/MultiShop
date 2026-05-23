using Microsoft.AspNetCore.Mvc;
using MultiShop.Payment.Dtos;
using System.Net.Http.Json;


namespace MultiShop.Payment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private static readonly HashSet<string> AllowedPaymentMethods = new(StringComparer.OrdinalIgnoreCase)
        {
            "CreditCard",
            "Paypal",
            "BankTransfer",
            "UsePoints"
        };

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CompletePayment([FromQuery] int addressId, [FromQuery] string paymentMethod)
        {
            try
            {
                if (addressId <= 0)
                {
                    return BadRequest("A valid addressId is required to complete payment.");
                }

                var selectedPaymentMethod = GetSupportedPaymentMethod(paymentMethod);

                if (string.IsNullOrWhiteSpace(selectedPaymentMethod))
                {
                    return BadRequest("A valid paymentMethod is required to complete payment.");
                }

                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized("Authorization token was not found in the payment request.");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));

            // Basket al
            var basketResponse = await _httpClient.GetAsync("http://localhost:7074/api/baskets");

            if (!basketResponse.IsSuccessStatusCode)
            {
                var basketError = await basketResponse.Content.ReadAsStringAsync();
                return StatusCode((int)basketResponse.StatusCode, $"Basket request failed: {basketError}");
            }

            var basket = await basketResponse.Content.ReadFromJsonAsync<BasketTotalDto>();

            if (basket == null || basket.BasketItems == null || !basket.BasketItems.Any())
            {
                return BadRequest("Basket empty");
            }

            // Ordering oluştur
            var ordering = new CreateOrderingDto
            {
                UserId = basket.UserId,
                TotalPrice = basket.TotalPrice,
                OrderDate = DateTime.Now,
                PaymentMethod = selectedPaymentMethod,
                AddressId = addressId
            };

            var orderingResponse = await _httpClient.PostAsJsonAsync("http://localhost:7072/api/orderings", ordering);

            if (!orderingResponse.IsSuccessStatusCode)
            {
                var orderingError = await orderingResponse.Content.ReadAsStringAsync();
                return StatusCode((int)orderingResponse.StatusCode, $"Ordering request failed: {orderingError}");
            }

            var createdOrder = await orderingResponse.Content.ReadFromJsonAsync<ResultOrderingDto>();

            if (createdOrder == null || createdOrder.OrderingId == 0)
            {
                var rawOrderingResponse = await orderingResponse.Content.ReadAsStringAsync();
                return BadRequest($"Ordering was created but OrderingId could not be read. Response: {rawOrderingResponse}");
            }

            // OrderDetail oluştur
            foreach (var item in basket.BasketItems)
            {
                var detail = new CreateOrderDetailDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    ProductTotalPrice = item.Quantity * item.UnitPrice,
                    OrderingId = createdOrder.OrderingId,
                    VendorId = item.VendorId
                };

                var detailResponse = await _httpClient.PostAsJsonAsync("http://localhost:7072/api/orderdetails", detail);

                if (!detailResponse.IsSuccessStatusCode)
                {
                    var detailError = await detailResponse.Content.ReadAsStringAsync();
                    return StatusCode((int)detailResponse.StatusCode, $"OrderDetail request failed: {detailError}");
                }
            }

            
            // Basket temizle
            var deleteBasketResponse = await _httpClient.DeleteAsync("http://localhost:7074/api/baskets");

            if (!deleteBasketResponse.IsSuccessStatusCode)
            {
                var deleteBasketError = await deleteBasketResponse.Content.ReadAsStringAsync();
                return StatusCode((int)deleteBasketResponse.StatusCode, $"Basket delete request failed: {deleteBasketError}");
            }

                return Ok(new
                {
                    Message = "Payment completed & order created",
                    OrderingId = createdOrder.OrderingId,
                    TotalPrice = basket.TotalPrice
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Payment service crashed: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }
        }

        private static string? GetSupportedPaymentMethod(string paymentMethod)
        {
            return AllowedPaymentMethods.FirstOrDefault(x =>
                string.Equals(x, paymentMethod, StringComparison.OrdinalIgnoreCase));
        }
    }
}
