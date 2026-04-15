using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    //UserController oluşturularak, kullanıcının profil sayfasına girdiğinde (Index metodu)
    // bu servisin tetiklenmesi ve verilerin View katmanına taşınması sağlandı.
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var values= await _userService.GetUserInfo();
            return View(values);
        }
        /*
         İstek Yolculuğu (Adım Adım)

        1)Tetikleme: GetUserInfo metodu içinde _httpClient.GetFromJsonAsync çağrılır.

        2)Handler Devreye Girer: İstek daha yola çıkmadan ResourceOwnerPasswordTokenHandler içindeki SendAsync metodu tetiklenir.
  
        3)Bilet Yapıştırma: Handler, kullanıcının o anki AccessToken'ını alır ve isteğin kafasına (Header) "Bearer [Token]" olarak yapıştırır.
        
        4)İsteği Gönderme: Handler içindeki base.SendAsync komutuyla istek gerçek hedefe (IdentityServer/User API) gönderilir.
        
        5)Geri Dönüş: Eğer karşı taraf "Biletin süresi dolmuş (401)" derse, Handler bunu yakalar, GetRefreshToken ile bileti yeniler ve isteği otomatik olarak tekrar gönderir.
        
        6)Final: Her şey tamamlandığında temiz veri UserService'e döner.
         */
    }
}
