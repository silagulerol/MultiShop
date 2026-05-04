using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class ProductController : Controller
    {
        private string GetCurrentVendorId()
        {
            return User.FindFirst("sub")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private async Task<bool> IsProductOwner(string productId)
        {
            var product = await _productService.GetByIdProductAsync(productId);

            if (product == null)
            {
                return false;
            }

            return product.VendorId == GetCurrentVendorId();
        }

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ProductViewBagList();

            var vendorId = GetCurrentVendorId();
            var values = await _productService.GetProductsByVendorIdAsync(vendorId);

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            var vendorId = GetCurrentVendorId();
            var values = await _productService.GetProductsWithCategoryByVendorIdAsync(vendorId);

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            // We are converting List<ResultCategoryDto> to  List<SelectListItem>.
            List<SelectListItem> categoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.Name,
                                                       Value = x.CategoryId
                                                   }).ToList();
            ViewBag.CategoryValues = categoryValues;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            createProductDto.VendorId = GetCurrentVendorId();

            await _productService.CreateProductAsync(createProductDto);

            return Redirect("/Vendor/Product/Index");
        }

        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (!await IsProductOwner(id))
            {
                return Forbid();
            }

            await _productService.DeleteProductAsync(id);
            return Redirect("/Vendor/Product/Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            if (!await IsProductOwner(id))
            {
                return Forbid();
            }

            var values = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> categoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.Name,
                                                       Value = x.CategoryId
                                                   }).ToList();
            ViewBag.CategoryValues = categoryValues;

            var value = await _productService.GetByIdProductAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            if (!await IsProductOwner(updateProductDto.ProductId))
            {
                return Forbid();
            }

            updateProductDto.VendorId = GetCurrentVendorId();

            await _productService.UpdateProductAsync(updateProductDto);
            return Redirect("/Vendor/Product/GetProductsWithCategory");
        }

        void ProductViewBagList()
        {
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";
            ViewBag.v0 = "Ürün İşlemleri";
        }
    }
}
