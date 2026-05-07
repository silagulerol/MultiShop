using MultiShop.DtoLayer.CargoDtos.CargoDetailDtos;

namespace MultiShop.WebUI.Services.CargoServices.CargoDetailServices
{
    public interface ICargoDetailService
    {
        Task<ResultCargoDetailDto> CreateCargoDetailAsync(CreateCargoDetailDto dto);
        Task<ResultCargoDetailDto?> GetByOrderDetailIdAsync(int orderDetailId);
        Task<bool> HasCargoAsync(int orderDetailId);
    }
}