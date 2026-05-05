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
            await _paymentService.CompletePaymentAsync();
            return Redirect("/User/MyOrder/MyOrderList" );
        }
    }
}