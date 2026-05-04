using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.Vendor.ViewComponents.VendorLayoutViewComponents
{
    public class _VendorLayoutSideBarComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
