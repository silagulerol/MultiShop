using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.OfferDiscountDtos;
using MultiShop.Catalog.Services.OfferDiscountService;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferDiscountsController : ControllerBase
    {

        private readonly IOfferDiscountService _offerDiscount;

        public OfferDiscountsController(IOfferDiscountService offerDiscount)
        {
            _offerDiscount = offerDiscount;
        }

        [HttpGet]
        public async Task<IActionResult> ListOfferDiscounts()
        {
            var values = await _offerDiscount.GetAllOfferDiscountAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfferDiscountById(string id)
        {
            var value = await _offerDiscount.GetByIdOfferDiscountAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOfferDiscountSlide(CreateOfferDiscountDto createOfferDiscountDto)
        {
            await _offerDiscount.CreateOfferDiscountAsync(createOfferDiscountDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto updateOfferDiscountDto)
        {
            await _offerDiscount.UpdateOfferDiscountAsync(updateOfferDiscountDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOfferDiscountSlide(string id)
        {
            await _offerDiscount.DeleteOfferDiscountAsync(id);
            return Ok();
        }

    }
}
