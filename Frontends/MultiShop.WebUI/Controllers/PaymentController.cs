using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.PaymentServices;

namespace MultiShop.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "Shop";
            ViewBag.directory2 = "Payment";
            ViewBag.directory3 = "Checkout";
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Index(string cardNumber, string cardHolder, string month, string year, string cvv)
        {
            try
            {
                var result = await _paymentService.CompletePaymentAsync();

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
    } 
}