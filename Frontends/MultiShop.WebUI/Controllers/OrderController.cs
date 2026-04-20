using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            OrderViewBag();
            return View();
        }
        public void OrderViewBag()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Orders";
            ViewBag.directory3 = "Order Operations";
        }
    }
}
