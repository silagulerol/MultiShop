using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoOperationDetailDtos;
using MultiShop.Cargo.EntityLayer.Concrete;
using System;
using System.Linq;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOperationController : ControllerBase
    {
        private readonly ICargoOperationService _service;

        public CargoOperationController(ICargoOperationService cargoOperationService)
        {
            _service = cargoOperationService;
        }

        [HttpGet]
        public IActionResult GetCargoOperationList()
        {
            var values = _service.TGetAll();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoOperationById(int id)
        {
            var value = _service.TGetById(id);

            if (value == null)
            {
                return NotFound("Cargo operation not found.");
            }

            return Ok(value);
        }

        [HttpGet("GetByCargoDetailId/{cargoDetailId}")]
        public IActionResult GetByCargoDetailId(int cargoDetailId)
        {
            var values = _service.TGetByCargoDetailId(cargoDetailId);

            return Ok(values);
        }

        [HttpPost]
        public IActionResult AddCargoOperation(CreateCargoOperationDto createCargoOperationDto)
        {
            var cargoOperation = new CargoOperation
            {
                CargoDetailId = createCargoOperationDto.CargoDetailId,
                Status = string.IsNullOrWhiteSpace(createCargoOperationDto.Status)
                    ? "Preparing"
                    : createCargoOperationDto.Status,
                Description = createCargoOperationDto.Description,
                OperationDate = createCargoOperationDto.OperationDate == default
                    ? DateTime.Now
                    : createCargoOperationDto.OperationDate
            };

            _service.TInsert(cargoOperation);
            return Ok("Cargo operation created successfully.");
        }

        [HttpPut]
        public IActionResult UpdateCargoOperation(UpdateCargoOperationDto updateCargoOperationDto)
        {
            var cargoOperation = new CargoOperation
            {
                CargoOperationId = updateCargoOperationDto.CargoOperationId,
                CargoDetailId = updateCargoOperationDto.CargoDetailId,
                Status = updateCargoOperationDto.Status,
                Description = updateCargoOperationDto.Description,
                OperationDate = updateCargoOperationDto.OperationDate
            };

            _service.TUpdate(cargoOperation);
            return Ok("Cargo operation updated successfully.");
        }

        [HttpDelete]
        public IActionResult DeleteCargoOperation(int id)
        {
            _service.TDelete(id);
            return Ok("Cargo operation deleted successfully.");
        }
    }
}
