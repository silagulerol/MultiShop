using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureSliderController : Controller
    {
        private readonly IFeatureSliderService _featureSliderService;

        public FeatureSliderController(IFeatureSliderService featureSliderService)
        {
            _featureSliderService = featureSliderService;
        }
        
        public async Task<IActionResult> Index()
        {
            
            FeatureSliderViewbagList();
            var values = await _featureSliderService.GetAllFeatureSlidersAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFeatureSlider()
        {
            FeatureSliderViewbagList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeatureSlider(CreateFeatureSliderDto createFeatureSliderDto)
        {
            FeatureSliderViewbagList();
            await _featureSliderService.InsertFeatureSliderAsync(createFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        public async Task<IActionResult> DeleteFeatureSlider(string id)
        {
            FeatureSliderViewbagList();
            await _featureSliderService.DeleteFeatureSliderAsync(id);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }


        [HttpGet]
        public async Task<IActionResult> UpdateFeatureSlider(string id)
        {
            FeatureSliderViewbagList();
            var values = await _featureSliderService.GetFeatureSliderByIdAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeatureSlider(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            FeatureSliderViewbagList();
            await _featureSliderService.UpdateFeatureSliderAsync(updateFeatureSliderDto);
            return RedirectToAction("Index", "FeatureSlider", new { area = "Admin" });
        }

        void FeatureSliderViewbagList()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "FeatureSliders";
            ViewBag.v3 = "Feature Slider List";
            ViewBag.v0 = "Feature Slider Operation";
        }
    }
}
