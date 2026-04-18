using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Pages";
            ViewBag.directory3 = "Products";
            return View();
        }
    }
}
