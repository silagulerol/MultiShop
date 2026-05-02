using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.DtoLayer.IdentityDtos.LogInDtos;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MultiShop.WebUI.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<bool> SignIn(SignInDto signInDto)
         {
            // GetDiscoveryDocumentAsync: Uygulamanın, IdentityServer'ın hangi kurallarla ve hangi adreslerle çalıştığını öğrenmesini sağlar.
            // normalde bu method https ile çalışır ama biz http ile çalıştığımız için hata verecektir. Bu yüzden Method parametrelerine yeni bir obje oluşturup içine http'yi ekliyoruz.
            var discoveryEndPoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.MultiShopManagerClient.ClientId,
                ClientSecret = _clientSettings.MultiShopManagerClient.ClientSecret,
                UserName = signInDto.UserName,
                Password = signInDto.Password,
                Address = discoveryEndPoint.TokenEndpoint
                // Address: burada http://localhost/5001/connect/token adresine istek atılacak. Bu adres IdentityServer'ın token alma endpoint'idir.
            };

            // IdentityServer her şey doğruysa içinde kullanıcının yetkilerinin olduğu bir Token dönüyor.
            /* token (Access Token): Bu bir yetki (authorization) aracıdır. 
             * Bizim API'lere (örneğin Katalog veya Sipariş mikroservisine) istek atarken 
             * "Benim bu kapıdan geçmeye iznim var" demek için kullandığımız anahtardır. Genelde şifreli bir metin yığınıdır.        */
            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            /* elindeki Access Token (erişim anahtarı) ile IdentityServer’ın kapısını tekrar çalıp,
             * "Bu anahtarın sahibi olan kullanıcının gerçek bilgileri (Adı, Soyadı, Email vb.) nelerdir?" sorusunu sorma işlemini yapar */
            var userInfoRequest = new UserInfoRequest
            {
                Address = discoveryEndPoint.UserInfoEndpoint,
                Token = token.AccessToken
            };

            var userValues = await _httpClient.GetUserInfoAsync(userInfoRequest);


            /* Bu kod bloğu, aslında tüm sürecin "Final" kısmıdır. IdentityServer'dan aldığın ham verileri (tokenlar ve kullanıcı bilgileri) alıp,
             * bunları tarayıcıda saklanacak bir Giriş Oturumuna (Session/Cookie) dönüştürüyorsun.        */
            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(userValues.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            var claims = userValues.Claims.ToList();

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);

            var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "role");

            if (roleClaim != null)
            {
                claims.RemoveAll(x => x.Type == "role");
                claims.Add(new Claim("role", roleClaim.Value));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                "name",
                "role"
            );

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            // IdentityServer'dan aldığın AccessToken (giriş anahtarı), RefreshToken (anahtar eskidiğinde yenisini alma hakkı)
            // ve ExpiresIn (süre) bilgilerini bu paketin içine koyuyorsun.
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = token.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },

                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString()
                }
            });

            authenticationProperties.IsPersistent = false;

            //1. Hazırladığın tüm bilgileri (Kimlik + Tokenlar) alır.
            //2. Bunları karmaşık ve güvenli bir şekilde şifreler.
            //3. Kullanıcının tarayıcısına bir **Cookie(Çerez) * *olarak gönderir.
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal, //hazırladığımız kimlik bilgileri, 
                authenticationProperties); //Tokenlar

            return true;
        }

        public async Task<bool> GetRefreshToken()
        {
            var discoveryEndPoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = _clientSettings.MultiShopManagerClient.ClientId,
                ClientSecret = _clientSettings.MultiShopManagerClient.ClientSecret,
                RefreshToken = refreshToken,
                Address = discoveryEndPoint.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            var authenticationToken = new List<AuthenticationToken>()
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = token.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString()
                }
            };

            var result = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var properties = result.Properties;

            properties.StoreTokens(authenticationToken);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Principal, properties);
            return true;
        }

    }
}
