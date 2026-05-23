using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOrderingServices;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;

namespace MultiShop.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class MyOrderController : Controller
    {
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IUserService _userService;
        private readonly IOrderDetailService _orderDetailService;

        public MyOrderController(IOrderOrderingService orderOrderingService, IUserService userService,IOrderDetailService orderDetailService)
        {
            _orderOrderingService = orderOrderingService;
            _userService = userService;
            _orderDetailService = orderDetailService;
        }
        public async Task<IActionResult> MyOrderList()
        {
            //Giriş yapan kullanıcıya ait verileri getirmek için gerekli işlemler yapılır. Örneğin, kullanıcı ID'si ile veritabanından siparişler çekilir.
            var user = await _userService.GetUserInfo();
            var values= await _orderOrderingService.GetOrderingByUserId(user.Id);
            return View(values);
        }

        public async Task<IActionResult> MyOrderDetail(int id)
        {
            var values = await _orderDetailService.GetOrderDetailsByOrderingIdAsync(id);
            return View(values);
        }
    }
}
