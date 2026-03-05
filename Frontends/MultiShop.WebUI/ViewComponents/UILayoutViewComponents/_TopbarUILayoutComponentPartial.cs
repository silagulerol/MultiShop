using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents._UILayoutViewComponents
{
    public class _TopbarUILayoutComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
