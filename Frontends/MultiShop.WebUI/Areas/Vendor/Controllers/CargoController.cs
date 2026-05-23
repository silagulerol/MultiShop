using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CargoDtos.CargoCompanyDtos;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;
using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;
using MultiShop.DtoLayer.CargoDtos.CargoOperationDtos;
using MultiShop.WebUI.Services.CargoServices.CargoDetailServices;
using MultiShop.WebUI.Services.CargoServices.CargoOperationServices;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    public class CargoController : Controller
    {
        private readonly ICargoCompanyService _cargoCompanyService;
        private readonly ICargoDetailService _cargoDetailService;
        private readonly ICargoOperationService _cargoOperationService;


        public CargoController(ICargoCompanyService cargoCompanyService, ICargoDetailService cargoDetailService, ICargoOperationService cargoOperationService)
        {
            _cargoCompanyService = cargoCompanyService;
            _cargoDetailService = cargoDetailService;
            _cargoOperationService = cargoOperationService;
        }

        public async Task<IActionResult> CargoCompanyList()
        {
            var values = await _cargoCompanyService.GetAllCargoCompanyAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCargoCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCargoCompany(CreateCargoCompanyDto createCargoCompanyDto)
        {
            await _cargoCompanyService.CreateCargoCompanyAsync(createCargoCompanyDto);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Vendor" });
        }


        public async Task<IActionResult> DeleteCargoCompany(int id)
        {
            await _cargoCompanyService.DeleteCargoCompanyAsync(id);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Vendor" });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCargoCompany(int id)
        {
            var values = await _cargoCompanyService.GetByIdCargoCompanyAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCargoCompany(UpdateCargoCompanyDto updateCargoCompanyDto)
        {
            await _cargoCompanyService.UpdateCargoCompanyAsync(updateCargoCompanyDto);
            return RedirectToAction("CargoCompanyList", "Cargo", new { Area = "Vendor" });
        }
    
        [HttpPost]
        public async Task<IActionResult> CreateShipment(int orderDetailId, int cargoCompanyId)
        {
            var hasCargo = await _cargoDetailService.HasCargoAsync(orderDetailId);

            if (hasCargo)
            {
                TempData["Error"] = "Shipment already created.";
                return Redirect("/Vendor/Order/OrderDetailList");
            }

            var vendorId = User.FindFirst("sub")?.Value;

            await _cargoDetailService.CreateCargoDetailAsync(new CreateCargoDetailDto
            {
                OrderDetailId = orderDetailId,
                VendorId = vendorId,
                CargoCompanyId = cargoCompanyId,
                CargoStatus = "Preparing"
            });

            return Redirect("/Vendor/Order/OrderDetailList");
        }
    
        [HttpPost]
        public async Task<IActionResult> UpdateShipmentStatus(int orderDetailId, string cargoStatus)
        {
            var cargo = await _cargoDetailService.GetByOrderDetailIdAsync(orderDetailId);

            if (cargo == null)
            {
                TempData["Error"] = "Shipment not found.";
                return Redirect("/Vendor/Order/OrderDetailList");
            }

            await _cargoOperationService.CreateCargoOperationAsync(new CreateCargoOperationDto
            {
                CargoDetailId = cargo.CargoDetailId,
                Status = cargoStatus,
                Description = $"Shipment status updated to {cargoStatus}",
                OperationDate = DateTime.Now
            });
            return Redirect("/Vendor/Order/OrderDetailList");
        } 
    }
}
