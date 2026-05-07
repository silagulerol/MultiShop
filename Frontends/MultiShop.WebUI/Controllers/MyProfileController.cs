using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOrderingServices;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;
using MultiShop.WebUI.Services.CargoServices.CargoDetailServices;
namespace MultiShop.WebUI.Controllers
{
    [Authorize]
    public class MyprofileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICargoDetailService _cargoDetailService;


        public MyprofileController(
            IUserService userService,
            IOrderOrderingService orderOrderingService, 
            IOrderDetailService orderDetailService,
            ICargoDetailService cargoDetailService)
        {
            _userService = userService;
            _orderOrderingService = orderOrderingService;
            _orderDetailService = orderDetailService;
            _cargoDetailService = cargoDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userService.GetUserInfo();
            var orders = await _orderOrderingService.GetOrderingByUserId(user.Id);

            ViewBag.UserEmail = user.Email;
            ViewBag.UserName = user.UserName;
            ViewBag.UserName = user.UserName;
            ViewBag.UserFullName = $"{user.Name} {user.Surname}";

            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            // 1. Order bilgisi
            var user = await _userService.GetUserInfo();
            var userOrders = await _orderOrderingService.GetOrderingByUserId(user.Id);
            var order = userOrders.FirstOrDefault(x => x.OrderingId == id);

            if (order == null)
            {
                return NotFound();
            }

            // 2. Order detail listesi
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderingIdAsync(id);

            // 3. Her orderDetail için cargo bilgisi çek
            var cargoDetails = new List<ResultCargoDetailDto>();

            foreach (var item in orderDetails)
            {
                var cargo = await _cargoDetailService.GetByOrderDetailIdAsync(item.OrderDetailId);

                if (cargo != null)
                {
                    cargoDetails.Add(cargo);
                }
            }

            ViewBag.Order = order;
            ViewBag.CargoDetails = cargoDetails;

            return View(orderDetails);
        }
    }
}