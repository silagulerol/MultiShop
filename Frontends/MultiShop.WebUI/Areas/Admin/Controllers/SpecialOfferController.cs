using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialOfferController : Controller
    {
        private readonly ISpecialOfferService _specialOfferService;

        public SpecialOfferController(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        public async Task<IActionResult> Index()
        {
            SpecialOfferViewbagList();
            var values = await _specialOfferService.GetAllSpecialOffersAsync();
           return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateSpecialOffer()
        {
            SpecialOfferViewbagList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto createSpecialOfferDto)
        {
            SpecialOfferViewbagList();
            await _specialOfferService.InsertSpecialOfferAsync(createSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });   
        }

        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            SpecialOfferViewbagList();
            await _specialOfferService.DeleteSpecialOfferAsync(id);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }


        [HttpGet]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {
            SpecialOfferViewbagList();
            var value = await _specialOfferService.GetSpecialOfferByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            SpecialOfferViewbagList();
            await _specialOfferService.UpdateSpecialOfferAsync(updateSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });
        }

        void SpecialOfferViewbagList()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "SpecialOffers";
            ViewBag.v3 = "SpecialOffer List";
            ViewBag.v0 = "SpecialOffer Operation";
        }
    }
}
