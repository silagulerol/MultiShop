using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
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

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderDetailId, string orderStatus)
        {
            var vendorId = GetCurrentVendorId();
            var orderDetails = await _orderDetailService.GetOrderDetailsByVendorIdAsync(vendorId);

            var detail = orderDetails.FirstOrDefault(x => x.OrderDetailId == orderDetailId);

            if (detail == null)
            {
                return Redirect("/Vendor/Order/OrderDetailList");
            }

            await _orderDetailService.UpdateOrderDetailAsync(new UpdateOrderDetailDto
            {
                OrderDetailId = detail.OrderDetailId,
                ProductId = detail.ProductId,
                ProductName = detail.ProductName,
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                ProductTotalPrice = detail.ProductTotalPrice,
                OrderingId = detail.OrderingId,
                VendorId = detail.VendorId,
                OrderStatus = orderStatus
            });

            return Redirect("/Vendor/Order/OrderDetailList");
        }
    }
}