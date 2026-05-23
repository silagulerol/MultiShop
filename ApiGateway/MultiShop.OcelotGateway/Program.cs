using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);



/* Bir ASP.NET Core uygulamas�n�n (senin senaryonda muhtemelen Ocelot Gateway projesinin), 
 * gelen isteklerdeki JWT (JSON Web Token) biletlerini nas�l do�rulayaca��n� belirleyen g�venlik konfig�rasyonudur.
 * bir binan�n giri�indeki "Otomatik Bilet Kontrol Sistemi"ni kurmak gibi */

//Uygulamaya "Varsay�lan kimlik do�rulama y�ntemimiz JWT Bearer (Bileti ta��yan getirir) sistemidir" der.
builder.Services.AddAuthentication()
    //Biletin ge�erli say�lmas� i�in hangi �artlar�n gerekti�ini detayland�r�r.
    //Bu sat�r, bu g�venlik ayarlar�n�n ad�n� "OcelotAuthenticationScheme" olarak belirler.
    //Neden �nemli? Ocelot'un ocelot.json dosyas�ndaki AuthenticationProviderKey alan�nda tam olarak bu ismi yazman gerekir.
    //E�er bu ismi vermezsen Ocelot hangi bilet kontrol cihaz�n� kullanaca��n� bilemez.
    .AddJwtBearer("OcelotAuthenticationScheme", options =>
    {
        //G�venilir Kaynak (Authority): Bileti kimin da��tt���n� (IdentityServer) belirler.
        //Mant�k: "E�er biletin �zerinde http://localhost:5001 (IdentityServer) imzas� yoksa,
        //bu bileti sahte kabul et ve kimseyi i�eri alma" demektir.Uygulama, biletin do�rulu�unu teyit etmek i�in bu adrese gider.
        options.Authority = builder.Configuration["IdentityServerUrl"];

        //Hedef Kitle (Audience)
        //Bu biletin hangi "oda" veya "servis" i�in kesildi�ini kontrol eder.
        //Mant�k: Biletin �zerinde "Bu bilet ResourceOcelot (Ocelot Gateway) i�in ge�erlidir" yazmas� gerekir.
        //E�er bilet ba�ka bir API(�rne�in sadece ResourceCatalog) i�in kesilmi�se, Ocelot bunu kabul etmez.
        options.Audience = "ResourceOcelot";

        //G�venlik bilgilerinin transferi i�in https protokol� zorunlulu�unu kald�r�r.
        options.RequireHttpsMetadata = false;

        /* Bu kod sayesinde uygulama �u �� soruyu sorar:
        Bu yap� kurulduktan sonra s�re� ��yle i�ler:
        1) �stek Gelir: Kullan�c� Postman �zerinden bir istek atar.
        2) Ocelot Yakalar: ocelot.json dosyas�na bakar ve bu rotan�n bir kimlik do�rulamas� istedi�ini g�r�r.
        3) �ema Kontrol�: Dosyada yazan "OcelotAuthenticationScheme" ismini senin bu kodunla e�le�tirir.
        4)Do�rulama:
            -Bilet IdentityServer taraf�ndan m� imzalanm��? (Authority)
            -Biletin hedefi buras� m�? (Audience)
        5)Karar: E�er her iki soruya da "Evet" cevab� gelirse iste�i mikroservise y�nlendirir, yoksa kap�dan �evirir.
                �zetle: Bu kod, uygulaman�n �n�ne bir koruma kalkan� koyar. Ge�erli bir bileti olmayan hi� kimse (401 Unauthorized hatas� alarak) arkadaki mikroservislerine ula�amaz. 
        
         Neden Bir �sim Vermek Zorunday�z?
            Ger�ek projelerde bazen birden fazla kimlik do�rulama y�ntemi olabilir:
            -Baz� kap�lar JWT (Dijital Bilet) ile a��l�r.
            -Baz� kap�lar ApiKey (�zel �ifre) ile a��l�r.
            -Baz� kap�lar Google Login ile a��l�r.
            E�er hepsine bir isim vermezsen, Ocelot hangi kap�da hangi "dedekt�r�" kullanaca��n� �a��r�r.
            �zetle: OcelotAuthenticationScheme ifadesi, senin kodunla konfig�rasyon dosyan (ocelot.json) aras�ndaki gizli el s�k��mad�r. Bu isimler birebir ayn� olmazsa, Ocelot "Ben bu kap�da kimlik kontrol� yapaca��m ama hangi kurallara g�re yapaca��m� (hangi cihaz� kullanaca��m�) bilmiyorum" der ve hata f�rlat�r.                     */
    });

/* Bu sat�rla programa �unu deriz: 
 "Senin ana ayar dosyan standart appsettings.json de�il, �zel olarak olu�turdu�um ocelot.json dosyas�d�r." */
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();

builder.Services.AddOcelot(configuration);

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

/*  Bu kod �al��t���nda proje bir "Trafik Polisi" gibi davranmaya ba�lar:

1) istek Gelir: Bir kullanıcı http://localhost:5000/services/catalog/categories adresine istek atar.
2) Ocelot Yakalar: UseOcelot katmanı bu isteği durdurur.
3) Dosyay� Kontrol Eder: ocelot.json i�ine bakar: "Biri /services/catalog/categories istedi, bunu nereye g�ndermeliyim?"
4) Y�nlendirir: Dosyada yazan ger�ek adrese (�rne�in http://localhost:7070/api/categories) iste�i paslar.
5) Cevab� D�ner: Mikroservisten gelen cevab� al�r ve kullan�c�ya geri iletir.
 */