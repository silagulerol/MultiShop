using MultiShop.DtoLayer.CatalogDtos.ContactDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ContactService
{
    public interface IContactService 
    {
        Task AddContactAsync(CreateContactDto createContactDto);
        Task DeleteContactAsync(string id);
        Task UpdateContactAsync(UpdateContactDto updateContactDto);
        Task<List<ResultContactDto>> ResultContactsAsync();
        Task<ResultContactDto> GetContactByIdAsync(string id);
    }
}
