using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.FeatureDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeaturedProductsComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _FeaturedProductsComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public  async Task<IViewComponentResult> InvokeAsync()
        {
            var values = (await _productService.GetAllProductAsync()).Take(5).ToList();
            return View(values);
        }
    }
}
