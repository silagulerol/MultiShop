using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.LogInDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class LogInController : Controller
    {
        private readonly IIdentityService _identityService;

        public LogInController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInDto signInDto)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var result = await _identityService.SignIn(signInDto);

            if (!result)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(signInDto);
            }

            return RedirectToAction("RedirectByRole");
        }

        [HttpGet]
        public IActionResult RedirectByRole()
        {
            var role = User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;

            
            if (role == "Admin")
            {
                return Redirect("/Admin/Product/Index");
            }

            if (role == "Vendor")
            {
                return RedirectToAction("Index", "Default");
            }

            if (role == "Customer")
            {
                return RedirectToAction("Index", "Default");
            }

            return RedirectToAction("Index", "Default");
        }
    }
}