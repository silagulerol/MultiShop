using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        //Task<List<ResultAddressDto>> GetAllAddressAsync();
        //Task UpdateAddressAsync(UpdateAddressDto updateAddressDto);
        Task<int> CreateAddressAsync(CreateAddressDto createAddressDto);
        Task<ResultAddressDto?> GetAddressByIdAsync(int id);
        Task<List<ResultAddressDto>> GetAddressesByUserIdAsync(string userId);
        //Task DeleteAddressAsync(string id);
        //Task<UpdateAddressDto> GetByIdAddressAsync(string id);
    }
}
