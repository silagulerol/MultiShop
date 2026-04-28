using MultiShop.DtoLayer.IdentityDtos.UserDtos;

namespace MultiShop.WebUI.Services.UserIdentityServices
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly HttpClient _httpClient;

        public UserIdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<ResultUserDto>> GetAllUserListAsync()
        {
            //var values = _httpClient.GetFromJsonAsync<List<ResultUserDto>>("http://localhost:5001/api/Users/GetAllUserList");
            var values = _httpClient.GetFromJsonAsync<List<ResultUserDto>>("/api/Users/GetAllUserList");
            return values;
        }
    }
}
