using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.CheckoutDtos;
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
        public async Task<IActionResult> Index()
        {
            OrderViewBag();

            var user = await _userService.GetUserInfo();
            var model = new CheckoutAddressDto
            {
                UseNewAddress = true,
                ExistingAddresses = await _orderAddressService.GetAddressesByUserIdAsync(user.Id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutAddressDto checkoutAddressDto)
        {
            //ViewComponent içinden index.cshtml'e taşıdık OrderAdddressViewComponent'i
            //o yüzden artık post request atan service'i controller içinde çağırıyoruz
            var value= await _userService.GetUserInfo();
            int addressId;
            if (!checkoutAddressDto.UseNewAddress && checkoutAddressDto.SelectedAddressId.HasValue)
            {
                var selectedAddress = await _orderAddressService.GetAddressByIdAsync(checkoutAddressDto.SelectedAddressId.Value);

                if (selectedAddress == null || selectedAddress.UserId != value.Id)
                {
                    return BadRequest("Selected address is not valid.");
                }

                addressId = checkoutAddressDto.SelectedAddressId.Value;
            }
            else
            {
                var createAddressDto = checkoutAddressDto.NewAddress;
                createAddressDto.UserId = value.Id;
                //descriptin şimdilik boş geçmesin diye
                createAddressDto.Description = "aa";
                addressId = await _orderAddressService.CreateAddressAsync(createAddressDto);
            }
           
            return RedirectToAction("Index", "Payment", new { addressId });
        }
        public void OrderViewBag()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Orders";
            ViewBag.directory3 = "Order Operations";
        }
    }
}
