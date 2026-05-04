using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferService;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class SpecialOfferController : Controller
    {
        private readonly ISpecialOfferService _specialOfferService;

        public SpecialOfferController(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        private string GetCurrentVendorId()
        {
            return User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private async Task<bool> IsSpecialOfferOwner(string specialOfferId)
        {
            var offer = await _specialOfferService.GetSpecialOfferByIdAsync(specialOfferId);

            if (offer == null)
                return false;

            return offer.VendorId == GetCurrentVendorId();
        }

        public async Task<IActionResult> Index()
        {
            SpecialOfferViewbagList();
            var vendorId = GetCurrentVendorId();
            var values = await _specialOfferService.GetSpecialOffersByVendorIdAsync(vendorId);
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
            createSpecialOfferDto.VendorId = GetCurrentVendorId();
            await _specialOfferService.InsertSpecialOfferAsync(createSpecialOfferDto);
            return Redirect("/Vendor/SpecialOffer/Index");   
        }

        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            if (!await IsSpecialOfferOwner(id))
                return Forbid();

            await _specialOfferService.DeleteSpecialOfferAsync(id);
            return Redirect("/Vendor/SpecialOffer/Index");
        }


        [HttpGet]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {
            if (!await IsSpecialOfferOwner(id))
                return Forbid();

            SpecialOfferViewbagList();
            var value = await _specialOfferService.GetSpecialOfferByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            if (!await IsSpecialOfferOwner(updateSpecialOfferDto.SpecialOfferId))
                return Forbid();

            updateSpecialOfferDto.VendorId = GetCurrentVendorId();

            await _specialOfferService.UpdateSpecialOfferAsync(updateSpecialOfferDto);
            return Redirect("/Vendor/SpecialOffer/Index");
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
