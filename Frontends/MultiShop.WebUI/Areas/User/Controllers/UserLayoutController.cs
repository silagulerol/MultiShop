using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.User.Controllers
{
    [Area("User")]
    //Kullanıcının profil, siparişler, mesajlar ve çıkış işlemlerini yöneteceği bir layout controller'ı. Bu controller, kullanıcıya özel sayfaların ortak bir görünümünü sağlar.
    public class UserLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
