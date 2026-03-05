using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.ViewComponents._UILayoutViewComponents
{
    public class _HeadUILayoutComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
