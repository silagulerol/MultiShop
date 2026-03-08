using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Catalog.Dtos.FeatureDtos;
using MultiShop.Catalog.Services.FeatureService;

namespace MultiShop.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeatureService _featuredService;

        public FeaturesController(IFeatureService featuredService)
        {
            _featuredService = featuredService;
        }

        [HttpGet]
        public async Task<IActionResult> ListFeatures()
        {
            var values = await _featuredService.GetAllFeatureAsync();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeatureById(string id)
        {
            var value = await _featuredService.GetByIdFeatureAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeatureSlide(CreateFeatureDto createFeatureDto)
        {
            await _featuredService.CreateFeatureAsync(createFeatureDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            await _featuredService.UpdateFeatureAsync(updateFeatureDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFeatureSlide(string id)
        {
            await _featuredService.DeleteFeatureAsync(id);
            return Ok();
        }

    }
}
