
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    /* Web UI projen bir mikroservise (Katalog, Sepet vb.) her istek attığında, bu sınıf araya girer 
     * ve isteğin üzerine kullanıcının giriş biletini (Access Token) otomatik olarak yapıştırır. 
     * Eğer biletin süresi dolmuşsa, kullanıcıya çaktırmadan bileti yeniler.     */


    //DelegatingHandler: Bu sınıf bir "Middleware" gibi çalışır ancak HTTP istekleri dışarı gönderilirken devreye girer.
    //Bir HttpClient üzerinden bir yere istek attığında, bu istek önce bu Handler'dan geçer.
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Kullanıcının tarayıcısındaki çerezden (Cookie) o anki geçerli AccessToken'ı çeker.
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
           
            //Bu token'ı request'in Authorization header'ına "Bearer [Token]" formatında ekler.
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
           
            //İstek artık yetkili bir şekilde yola çıkar.
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Diyelim ki istek mikroservise ulaştı ama mikroservis "Bu token'ın süresi dolmuş!" dedi
                //ve 401 hatası döndü. Kod burada durur ve durumu fark eder.
                //_identityService.GetRefreshToken() metodunu çağırarak IdentityServer'dan yeni bir Access Token alır.
                var tokenResponse = await _identityService.GetRefreshToken();

                if (tokenResponse != null)
                {
                    var newAccessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                    //Kullanıcıyı giriş sayfasına atmak yerine, arka planda yeni bileti alır, isteğin üzerine yapıştırır
                    //ve isteği tekrar gönderir. Kullanıcı biletinin süresinin dolduğunu fark etmez bile, işlem başarıyla sonuçlanır.
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //hata mesajı
            }
            return response;
        }
    }
}