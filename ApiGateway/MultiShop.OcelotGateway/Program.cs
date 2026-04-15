using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);



/* Bir ASP.NET Core uygulamasýnýn (senin senaryonda muhtemelen Ocelot Gateway projesinin), 
 * gelen isteklerdeki JWT (JSON Web Token) biletlerini nasýl doðrulayacaðýný belirleyen güvenlik konfigürasyonudur.
 * bir binanýn giriþindeki "Otomatik Bilet Kontrol Sistemi"ni kurmak gibi */

//Uygulamaya "Varsayýlan kimlik doðrulama yöntemimiz JWT Bearer (Bileti taþýyan getirir) sistemidir" der.
builder.Services.AddAuthentication()
    //Biletin geçerli sayýlmasý için hangi þartlarýn gerektiðini detaylandýrýr.
    //Bu satýr, bu güvenlik ayarlarýnýn adýný "OcelotAuthenticationScheme" olarak belirler.
    //Neden Önemli? Ocelot'un ocelot.json dosyasýndaki AuthenticationProviderKey alanýnda tam olarak bu ismi yazman gerekir.
    //Eðer bu ismi vermezsen Ocelot hangi bilet kontrol cihazýný kullanacaðýný bilemez.
    .AddJwtBearer("OcelotAuthenticationScheme", options =>
    {
        //Güvenilir Kaynak (Authority): Bileti kimin daðýttýðýný (IdentityServer) belirler.
        //Mantýk: "Eðer biletin üzerinde http://localhost:5001 (IdentityServer) imzasý yoksa,
        //bu bileti sahte kabul et ve kimseyi içeri alma" demektir.Uygulama, biletin doðruluðunu teyit etmek için bu adrese gider.
        options.Authority = builder.Configuration["IdentityServerUrl"];

        //Hedef Kitle (Audience)
        //Bu biletin hangi "oda" veya "servis" için kesildiðini kontrol eder.
        //Mantýk: Biletin üzerinde "Bu bilet ResourceOcelot (Ocelot Gateway) için geçerlidir" yazmasý gerekir.
        //Eðer bilet baþka bir API(örneðin sadece ResourceCatalog) için kesilmiþse, Ocelot bunu kabul etmez.
        options.Audience = "ResourceOcelot";

        //Güvenlik bilgilerinin transferi için https protokolü zorunluluðunu kaldýrýr.
        options.RequireHttpsMetadata = false;

        /* Bu kod sayesinde uygulama þu üç soruyu sorar:
        Bu yapý kurulduktan sonra süreç þöyle iþler:
        1) Ýstek Gelir: Kullanýcý Postman üzerinden bir istek atar.
        2) Ocelot Yakalar: ocelot.json dosyasýna bakar ve bu rotanýn bir kimlik doðrulamasý istediðini görür.
        3) Þema Kontrolü: Dosyada yazan "OcelotAuthenticationScheme" ismini senin bu kodunla eþleþtirir.
        4)Doðrulama:
            -Bilet IdentityServer tarafýndan mý imzalanmýþ? (Authority)
            -Biletin hedefi burasý mý? (Audience)
        5)Karar: Eðer her iki soruya da "Evet" cevabý gelirse isteði mikroservise yönlendirir, yoksa kapýdan çevirir.
                Özetle: Bu kod, uygulamanýn önüne bir koruma kalkaný koyar. Geçerli bir bileti olmayan hiç kimse (401 Unauthorized hatasý alarak) arkadaki mikroservislerine ulaþamaz. 
        
         Neden Bir Ýsim Vermek Zorundayýz?
            Gerçek projelerde bazen birden fazla kimlik doðrulama yöntemi olabilir:
            -Bazý kapýlar JWT (Dijital Bilet) ile açýlýr.
            -Bazý kapýlar ApiKey (Özel Þifre) ile açýlýr.
            -Bazý kapýlar Google Login ile açýlýr.
            Eðer hepsine bir isim vermezsen, Ocelot hangi kapýda hangi "dedektörü" kullanacaðýný þaþýrýr.
            Özetle: OcelotAuthenticationScheme ifadesi, senin kodunla konfigürasyon dosyan (ocelot.json) arasýndaki gizli el sýkýþmadýr. Bu isimler birebir ayný olmazsa, Ocelot "Ben bu kapýda kimlik kontrolü yapacaðým ama hangi kurallara göre yapacaðýmý (hangi cihazý kullanacaðýmý) bilmiyorum" der ve hata fýrlatýr.                     */
    });

/* Bu satýrla programa þunu deriz: 
 "Senin ana ayar dosyan standart appsettings.json deðil, özel olarak oluþturduðum ocelot.json dosyasýdýr." */
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();

builder.Services.AddOcelot(configuration);

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

/*  Bu kod çalýþtýðýnda proje bir "Trafik Polisi" gibi davranmaya baþlar:

1) Ýstek Gelir: Bir kullanýcý http://localhost:5000/services/catalog/categories adresine istek atar.
2) Ocelot Yakalar: UseOcelot katmaný bu isteði durdurur.
3) Dosyayý Kontrol Eder: ocelot.json içine bakar: "Biri /services/catalog/categories istedi, bunu nereye göndermeliyim?"
4) Yönlendirir: Dosyada yazan gerçek adrese (örneðin http://localhost:7070/api/categories) isteði paslar.
5) Cevabý Döner: Mikroservisten gelen cevabý alýr ve kullanýcýya geri iletir.
 */