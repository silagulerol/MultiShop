using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductYouLikeComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();

        }
    }
}
