using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace MultiShop.WebUI.Controllers
{
    //burada amaç visitor token üretip sonrasında bu syafaya erişim sağlayıp sağlayamayacağını test etmekti. 
    //Visitor token'ı ürettikten sonra bu sayfaya erişim sağlanırsa token'ın geçerli olduğunu ve yetkilerin doğru şekilde tanımlandığını görmüş oluruz.
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICategoryService _categoryService;

        public TestController(IHttpClientFactory httpClientFactory, ICategoryService categoryService)
        {
            _httpClientFactory = httpClientFactory;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            
            string token = ""; //Burada API’den gelecek access_token saklanacak.

            //API’ye istek atmak için kullanılıyor. using → iş bitince otomatik dispose eder(memory leak olmaz)
            using (var httpClient = new HttpClient()) // connect/token IdentityServer’ın token üretme endpoint’i
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://localhost:5001/connect/token"),
                    Method = HttpMethod.Post,
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"client_id", "MultiShopVisitorId" }, //Senin uygulamanın ID’si
                        {"client_secret", "multishopsecret"}, //Senin uygulamanın Şifresi
                        {"grant_type", "client_credentials" } //Senin uygulamanın hangi login yöntemi
                        //Bu şu demek: Kullanıcı yok. Sadece uygulama → uygulama konuşuyor. Örnek:
                        // Catalog API → IdentityServer’dan token alır. User login gerekmez 
                    })
                };
                using (var response = await httpClient.SendAsync(request))
                {
                      //Gelen JSON şöyle olur:
                      //{"access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
                      //    "expires_in": 3600,
                      //    "token_type": "Bearer"}
                  
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var tokenResponse = JObject.Parse(content); //JSON string → object
                        token = tokenResponse["access_token"].ToString(); //Artık elimde JWT token var
                    }
                }
            }

            var client = _httpClientFactory.CreateClient();
            // scehem olarak verdiğimiz Bearer, token'ın türünü belirtir. Postman'da authentication kısmından OAuth2 seçip token'ı yapıştırdığımızda otomatik olarak Bearer eklenir. Biz de burada manuel olarak ekliyoruz.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // isteğimizi ataca olan client'a token türünü ve token'ının Authorization propertysine ekeledik. Categories microservice'ine böylece token ile istedk atmış olduk.
            var responseMessage = await client.GetAsync("https://localhost:7070/api/Categories");
            
            if (responseMessage.IsSuccessStatusCode)
            {
                //Gelen response'un Conetntini okuduk ve içindeki jsondatayı ResultCategoryDto'ya çevirerek view' ilettik modeli.
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> Deneme()
        {
            // Bu metodun amacı, AddHttpClient ile eklediğimiz ICategoryService'in gerçekten token'ı alıp header'a ekleyip eklemediğini test etmektir.
            // Aynı zamanda bu metodun çalışması için öncelikle Visitor token'ı üretilmiş ve tarayıcıya yapıştırılmış olmalıdır. 
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
    }
}
