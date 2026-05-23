using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.PaymentServices;

namespace MultiShop.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private static readonly HashSet<string> AllowedPaymentMethods = new(StringComparer.OrdinalIgnoreCase)
        {
            "CreditCard",
            "Paypal",
            "BankTransfer",
            "UsePoints"
        };

        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Index(int addressId)
        {
            ViewBag.directory1 = "Shop";
            ViewBag.directory2 = "Payment";
            ViewBag.directory3 = "Checkout";
            ViewBag.AddressId = addressId;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Index(string cardNumber, string cardHolder, string month, string year, string cvv, int addressId, string paymentMethod)
        {
            var selectedPaymentMethod = GetSupportedPaymentMethod(paymentMethod);

            if (string.IsNullOrWhiteSpace(selectedPaymentMethod))
            {
                ViewBag.directory1 = "Shop";
                ViewBag.directory2 = "Payment";
                ViewBag.directory3 = "Checkout";
                ViewBag.AddressId = addressId;
                ModelState.AddModelError(nameof(paymentMethod), "Please select a valid payment method.");
                return View();
            }

            try
            {
                var result = await _paymentService.CompletePaymentAsync(addressId, selectedPaymentMethod);

                return RedirectToAction("Success", new
                { 
                    orderingId = result.OrderingId,
                    totalPrice = result.TotalPrice
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Fail", new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Success(int orderingId, decimal totalPrice)
        {
            ViewBag.OrderingId = orderingId;
            ViewBag.TotalPrice = totalPrice;
            return View();
        }

        [HttpGet]
        public IActionResult Fail(string message)
        {
            ViewBag.Error = message;
            return View();
        }

        private static string? GetSupportedPaymentMethod(string paymentMethod)
        {
            return AllowedPaymentMethods.FirstOrDefault(x =>
                string.Equals(x, paymentMethod, StringComparison.OrdinalIgnoreCase));
        }
    } 
}
