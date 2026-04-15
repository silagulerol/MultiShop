using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Services.Concrete
{
    public class UserService: IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDetailViewModel> GetUserInfo()
        {
            // UserService içinde GetFromJsonAsync kullanılarak kullanıcı bilgilerini almak için,
            // API katmanındaki kullanıcı endpoint'ine (/api/user/getuser) (identityServder'daki bir controller endpoint'i) asenkron bir istek atıldı.
            // Bu istek, ResourceOwnerPasswordTokenHandler tarafından otomatik olarak AccessToken ile donatılır. 
            return await _httpClient.GetFromJsonAsync<UserDetailViewModel>("api/Users/GetUser");
        }
    }
}
