using Microsoft.AspNetCore.SignalR;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MultiShop.Basket.LoginServices
{
    public class LoginService : ILoginService
    {
        /*
         HttpContext içinde request bilgileri vardır:  Headers, Token, User, Claims, Cookies
         Ama normal class içinde HttpContext'e direkt erişemezsin. Örnek: BasketController içinde HttpContext'e erişebilirsin çünkü o bir controller ve controller'lar HttpContext'e erişebilirler. 
         Ama LogIn Service class'tır ve HttpContext'e erişemez. O yüzden ASP.NET şunu verir: IHttpContextAccessor
         Bu sayede her yerden request bilgisi alınabilir.
         */
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        /*_httpContextAccessor.HttpContext : Bu current request.
         *
            HttpContext.User : Bu CLAIMSPRINCIPAL.
            JWT doğrulandıktan sonra ASP.NET token'ı claims olarak buraya koyar.
            Claim bul: FindFirst("sub") : JWT token içindeki:

              {
                 "sub": "1",
                 "name": "admin",
                 "email": "admin@mail.com"
               }

               Bu tokenın decode edilmiş hali
               Buradaki: sub --> subject = user id 
               Value al: .Value---> Sonuç: "1"*/

        //Bu bir property ve => şunu ifade ediyor: GetUserId çağrıldığında sağ taraftaki expression çalıştırılır ve sonucu döndürülür.
    }
}
