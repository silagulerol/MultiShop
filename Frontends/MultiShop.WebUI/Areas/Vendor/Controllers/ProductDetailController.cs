using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDetailDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductDetailService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet]
        // Buradaki id değeri productId'ye karşılık gelmektedir. 
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            var value = await _productDetailService.GetByProductIdProductDetailAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto updateProductDetailDto)
        {
            await _productDetailService.UpdateProductDetailAsync(updateProductDetailDto);
            return RedirectToAction("GetProductsWithCategory", "Product", new { area = "Admin" });
        }

        void ProductDetailViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Product Details";
            ViewBag.v3 = "Product Detail Update";
            ViewBag.v0 = "Product Detail Operation";
        }
        
    }
}
