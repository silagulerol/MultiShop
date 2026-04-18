using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.AboutDtos;
using MultiShop.WebUI.Services.CatalogServices.AboutService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            AboutViewBag();
            var values = await _aboutService.GetAllAboutAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAbout()
        {
            AboutViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutDto createAboutDto)
        {
            AboutViewBag();
            await _aboutService.CreateAboutAsync(createAboutDto);
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteAbout(string id)
        {
            AboutViewBag();
            await _aboutService.DeleteAboutAsync(id);
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }


        [HttpGet]
        public async Task<IActionResult> UpdateAbout(string id)
        {
            AboutViewBag();
            var values = await _aboutService.GetByIdAboutAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            AboutViewBag();
            await _aboutService.UpdateAboutAsync(updateAboutDto);
            return RedirectToAction("Index", "About", new { area = "Admin" });
        }

        void AboutViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Abouts";
            ViewBag.v3 = "About List";
            ViewBag.v0 = "About Operation";
        }
    }
}
