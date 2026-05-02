using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureDtos;
using MultiShop.WebUI.Services.CatalogServices.FeatureService;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            FeatureViewbagList();
            var values = await _featureService.GetAllFeatureAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFeature()
        {
            FeatureViewbagList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto createFeatureDto)
        {
            FeatureViewbagList();
            await _featureService.CreateFeatureAsync(createFeatureDto);
            return RedirectToAction("Index", "Feature", new { area = "Admin" });
            
        }

        public async Task<IActionResult> DeleteFeature(string id)
        {
            FeatureViewbagList();
            await _featureService.DeleteFeatureAsync(id);
            return RedirectToAction("Index", "Feature", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFeature(string id)
        {
            FeatureViewbagList();
            var value = await _featureService.GetByIdFeatureAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            FeatureViewbagList();
            await _featureService.UpdateFeatureAsync(updateFeatureDto);
            return RedirectToAction("Index", "Feature", new { area = "Admin" });
        }
        void FeatureViewbagList()
        {
            ViewBag.v1 = "Home Page";
            ViewBag.v2 = "Categories";
            ViewBag.v3 = "Category List";
            ViewBag.v0 = "Category Operations";
        }
    }
}
