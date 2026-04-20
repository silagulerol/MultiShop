using MultiShop.DtoLayer.CatalogDtos.ContactDtos;

namespace MultiShop.WebUI.Services.CatalogServices.ContactService
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      
        public async Task<List<ResultContactDto>> ResultContactsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ResultContactDto>>("contacts");
        }
        public async Task<ResultContactDto> GetContactByIdAsync(string id)
        {
           return await _httpClient.GetFromJsonAsync<ResultContactDto>($"contacts/{id}");
        }

        public async Task AddContactAsync(CreateContactDto createContactDto)
        {
            await _httpClient.PostAsJsonAsync("contacts", createContactDto);
        }

        public async Task UpdateContactAsync(UpdateContactDto updateContactDto)
        {
            await _httpClient.PutAsJsonAsync("contacts", updateContactDto);
        }
        public async Task DeleteContactAsync(string id)
        {
            await _httpClient.DeleteAsync($"contacts?id={id}");
        }
    }
}
