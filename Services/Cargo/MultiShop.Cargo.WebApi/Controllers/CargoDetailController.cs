using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoDetailDtos;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoDetailController : ControllerBase
    {
        private readonly ICargoDetailService _service;

        public CargoDetailController(ICargoDetailService cargoDetailService)
        {
            _service = cargoDetailService;
        }

        [HttpGet]
        public IActionResult GetCargoDetailList()
        {
            var values = _service.TGetAll();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoDetailById(int id)
        {
            var value = _service.TGetById(id);
            return Ok(value);
        }

        [HttpGet("GetByOrderDetailId/{orderDetailId}")]
        public IActionResult GetByOrderDetailId(int orderDetailId)
        {
            var value = _service.TGetByOrderDetailId(orderDetailId);

            if (value == null)
            {
                return NotFound("Cargo detail not found for this order detail.");
            }

            var result = new ResultCargoDetailDto
            {
                CargoDetailId = value.CargoDetailId,
                OrderDetailId = value.OrderDetailId,
                TrackingNumber = value.TrackingNumber,
                CargoStatus = value.CargoStatus,
                CargoCompanyId = value.CargoCompanyId,
                CargoCompanyName = value.CargoCompany?.CargoCompanyName
            };

            return Ok(result);
        }

        [HttpGet("GetByVendorId/{vendorId}")]
        public IActionResult GetByVendorId(string vendorId)
        {
            var values = _service.TGetByVendorId(vendorId);

            return Ok(values);
        }

        [HttpPost]
        public IActionResult AddCargoDetail(CreateCargoDetailDto createCargoDetailDto)
        {
            var cargoDetail = new CargoDetail
            {
                OrderDetailId = createCargoDetailDto.OrderDetailId,
                VendorId = createCargoDetailDto.VendorId,
                TrackingNumber = "TRK-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                CargoCompanyId = createCargoDetailDto.CargoCompanyId,
                CargoStatus = string.IsNullOrWhiteSpace(createCargoDetailDto.CargoStatus)
                    ? "Preparing"
                    : createCargoDetailDto.CargoStatus,
                CreatedDate = DateTime.Now
            };

            _service.TInsert(cargoDetail);

            return Ok(new ResultCargoDetailDto
            {
                CargoDetailId = cargoDetail.CargoDetailId,
                OrderDetailId = cargoDetail.OrderDetailId,
                TrackingNumber = cargoDetail.TrackingNumber,
                CargoStatus = cargoDetail.CargoStatus,
                CargoCompanyId = cargoDetail.CargoCompanyId
            });
        }

        [HttpPut]
        public IActionResult UpdateCargoDetail(UpdateCargoDetailDto updateCargoDetailDto)
        {
            var cargoDetail = new CargoDetail
            {
                CargoDetailId = updateCargoDetailDto.CargoDetailId,
                OrderDetailId = updateCargoDetailDto.OrderDetailId,
                VendorId = updateCargoDetailDto.VendorId,
                TrackingNumber = updateCargoDetailDto.TrackingNumber,
                CargoCompanyId = updateCargoDetailDto.CargoCompanyId,
                CargoStatus = updateCargoDetailDto.CargoStatus,
                CreatedDate = updateCargoDetailDto.CreatedDate
            };

            _service.TUpdate(cargoDetail);
            return Ok("Cargo detail updated successfully.");
        }

        [HttpDelete]
        public IActionResult DeleteCargoDetail(int id)
        {
            _service.TDelete(id);
            return Ok("Cargo detail deleted successfully.");
        }
    }
}
