using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.BasketServices;

namespace MultiShop.WebUI.ViewComponents.OrderViewComponents
{
    public class _OrderDetailComponentPartial : ViewComponent
    {
        private readonly IBasketService _basketService;

        public _OrderDetailComponentPartial(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basket = await _basketService.GetBasketAsync();
            return View(basket);
        }
    }
}
