using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<List<ResultOrderDetailDto>> GetOrderDetailsByVendorIdAsync(string vendorId);
        Task CreateOrderDetailAsync(CreateOrderDetailDto dto);
        Task<List<ResultOrderDetailDto>> GetOrderDetailsByOrderingIdAsync(int orderingId);
        
        Task UpdateOrderDetailAsync(UpdateOrderDetailDto updateOrderDetailDto);
    }
}