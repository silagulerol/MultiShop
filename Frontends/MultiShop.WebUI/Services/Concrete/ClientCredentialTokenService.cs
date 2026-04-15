using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Settings;
using System.Net;

namespace MultiShop.WebUI.Services.Concrete
{
    public class ClientCredentialTokenService: IClientCredentialTokenService
    {
        //url'lerimi tutan sınıf
        private readonly ServiceApiSettings _serviceApiSettings;
        // client'ın type'ını , secret'ını ve id'isni tutan sınıf
        private readonly HttpClient _httpClient;
        private readonly IClientAccessTokenCache  _clientAccessTokenCache;
        private readonly ClientSettings _clientSettings;

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, HttpClient httpClient, IClientAccessTokenCache clientAccessTokenCache, IOptions<ClientSettings> clientSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _httpClient = httpClient;
            this._clientAccessTokenCache = clientAccessTokenCache;
            _clientSettings = clientSettings.Value;
        }

        public async Task<string> GetToken()
        {
            // 1. Önce Cache'e (Belleğe) bak
            var currentToken = await _clientAccessTokenCache.GetAsync("multishoptoken");

            if (currentToken != null)
            {
                return currentToken.AccessToken;
            }

            // 2. Cache'de yoksa IdentityServer'ın adreslerini öğren (Discovery)
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps =false
                }
            });

            // 3. Client Credentials (Misafir) isteğini hazırla
            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.MultiShopVisitorClient.ClientId,
                ClientSecret = _clientSettings.MultiShopVisitorClient.ClientSecret,
                Address = discovery.TokenEndpoint
            };

            // 4. Yeni Token'ı al
            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            // 5. Yeni Token'ı belleğe kaydet (Süresi bitene kadar sakla)
            await _clientAccessTokenCache.SetAsync("multishoptoken", newToken.AccessToken, newToken.ExpiresIn);

            return newToken.AccessToken;
        }
    }
}
