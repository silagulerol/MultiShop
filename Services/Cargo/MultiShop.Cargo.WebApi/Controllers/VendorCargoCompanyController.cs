using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoOperationDetailDtos;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VendorCargoCompanyController : ControllerBase
    {
        private readonly IVendorCargoCompanyService _service;

        public VendorCargoCompanyController(IVendorCargoCompanyService service)
        {
            _service = service;
        }

        [HttpGet("GetByVendorId/{vendorId}")]
        public IActionResult GetByVendorId(string vendorId)
        {
            var values = _service.TGetByVendorId(vendorId);
            return Ok(values);
        }

        [HttpPost]
        public IActionResult Add(VendorCargoCompany entity)
        {
            _service.TInsert(entity);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _service.TDelete(id);
            return Ok();
        }
    }
}
