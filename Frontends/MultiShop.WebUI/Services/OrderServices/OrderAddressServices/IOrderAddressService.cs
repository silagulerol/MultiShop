using MultiShop.DtoLayer.OrderDtos.OrderAddressDtos;

namespace MultiShop.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        //Task<List<ResultAddressDto>> GetAllAddressAsync();
        //Task UpdateAddressAsync(UpdateAddressDto updateAddressDto);
        Task CreateAddressAsync(CreateAddressDto createAddressDto);
        //Task DeleteAddressAsync(string id);
        //Task<UpdateAddressDto> GetByIdAddressAsync(string id);
    }
}
