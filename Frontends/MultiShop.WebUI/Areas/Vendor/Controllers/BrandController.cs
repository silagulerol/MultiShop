using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.BrandDtos;
using MultiShop.WebUI.Services.CatalogServices.BrandService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            BrandViewbagList();
            var values = await _brandService.GetAllBrandAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBrand()
        {
            BrandViewbagList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto)
        {
            BrandViewbagList();
            await _brandService.CreateBrandAsync(createBrandDto);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteBrand(string id)
        {
            BrandViewbagList();
            await _brandService.DeleteBrandAsync(id);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBrand(string id)
        {
            BrandViewbagList();
            var values = await _brandService.GetByIdBrandAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto)
        {
            BrandViewbagList();
            await _brandService.UpdateBrandAsync(updateBrandDto);
            return RedirectToAction("Index", "Brand", new { area = "Admin" });
        }

        void BrandViewbagList()
        {
            ViewBag.v1 = "Home Page";
            ViewBag.v2 = "Categories";
            ViewBag.v3 = "Category List";
            ViewBag.v0 = "Category Operations";
        }
    }
}
