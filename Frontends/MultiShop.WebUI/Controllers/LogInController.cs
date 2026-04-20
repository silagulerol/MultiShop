using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MultiShop.DtoLayer.IdentityDtos.LogInDtos;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Controllers
{
    public class LogInController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIdentityService _identityService;

        public LogInController(IHttpClientFactory httpClientFactory, IIdentityService identityService)
        {
            _httpClientFactory = httpClientFactory;
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
            signInDto.UserName = "sam01";
            signInDto.Password = "1111aA*";
            await _identityService.SignIn(signInDto);
            return RedirectToAction("Index", "Default");
            //return View();
        }

        // Kullanıcı bilgilerini doğrulamak ve JWT token almak için giriş işlemi
        /*public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            signInDto.UserName = "sam01";
            signInDto.Password = "1111aA*";
            await _identityService.SignIn(signInDto);
            return RedirectToAction("Index", "User");
        }*/
    }
}
