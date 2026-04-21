using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;

namespace MultiShop.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderAddressService _orderAddressService;
        private readonly IUserService _userService;
        public OrderController(IOrderAddressService orderAddressService, IUserService userService)
        {
            _orderAddressService = orderAddressService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            OrderViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index( CreateAddressDto createAddressDto)
        {
            //ViewComponent içinden index.cshtml'e taşıdık OrderAdddressViewComponent'i
            //o yüzden artık post request atan service'i controller içinde çağırıyoruz
            var value= await _userService.GetUserInfo();
            createAddressDto.UserId = value.Id;

            //descriptin şimdilik boş geçmesin diye
            createAddressDto.Description = "aa";
            
            await _orderAddressService.CreateAddressAsync(createAddressDto);

            return RedirectToAction("Index", "Payment");
        }
        public void OrderViewBag()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Orders";
            ViewBag.directory3 = "Order Operations";
        }
    }
}
