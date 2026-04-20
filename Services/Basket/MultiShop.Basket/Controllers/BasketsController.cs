using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Basket.Dtos;
using MultiShop.Basket.LoginServices;
using MultiShop.Basket.Services;

namespace MultiShop.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IBasketService _basketService;
        public BasketsController(ILoginService loginService, IBasketService basketService)
        {
            _loginService = loginService;
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketDetailAsync() 
        {
            //bu bize sisteme girmiş olan token'a ait bilgileri(jwt.io da gördüğümüz datalar) vericek
            var user = User.Claims;
            var value = await _basketService.GetBasketAsync(_loginService.GetUserId);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBasketAsync(BasketTotalDto basketTotalDto)
        {
            basketTotalDto.UserId = _loginService.GetUserId;
            await _basketService.SaveBasketAsync(basketTotalDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketAsync()
        {
            await _basketService.DeleteBasketAsync(_loginService.GetUserId);
            return Ok();
        }

    }
}
