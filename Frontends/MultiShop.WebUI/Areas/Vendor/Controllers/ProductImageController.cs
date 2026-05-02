using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductImageDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductImageService;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            ProductImageViewBag();
            var values= await _productImageService.GetByProductIdProductImageAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> ProductImageDetail(UpdateProductImageDto updateProductImageDto)
        {
            ProductImageViewBag();
            await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            return RedirectToAction("GetProductsWithCategory", "Product", new { area = "Admin" });
        }

        void ProductImageViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product Images";
            ViewBag.v3 = "Product Image Update";
            ViewBag.v0 = "Product Image Operation";
        }
    }
}
