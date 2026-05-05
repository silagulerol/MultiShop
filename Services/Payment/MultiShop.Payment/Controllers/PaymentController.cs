using Microsoft.AspNetCore.Mvc;
using MultiShop.Payment.Dtos;
using System.Net.Http.Json;


namespace MultiShop.Payment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CompletePayment()
        {
            try
            {
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
                OrderDate = DateTime.Now
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

                return Ok("Payment completed & order created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Payment service crashed: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }
        }
    }
}
