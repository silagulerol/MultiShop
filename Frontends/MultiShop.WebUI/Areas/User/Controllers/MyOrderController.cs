using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class MyOrderController : Controller
    {
        public IActionResult MyOrderList()
        {
            //Giriş yapan kullanıcıya ait verileri getirmek için gerekli işlemler yapılır. Örneğin, kullanıcı ID'si ile veritabanından siparişler çekilir.

            return View();
        }
    }
}
