using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class OrderController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        private string GetCurrentVendorId()
        {
            return User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IActionResult> OrderDetailList()
        {
            var vendorId = GetCurrentVendorId();
            var values = await _orderDetailService.GetOrderDetailsByVendorIdAsync(vendorId);
            return View(values);
        }
    }
}