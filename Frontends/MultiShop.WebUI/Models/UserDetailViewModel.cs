using IdentityModel;

namespace MultiShop.WebUI.Models
{
    //Identity Server'dan dönecek kullanıcı verilerini karşılayacak olan "kap" sınıfı.
    //Genişletilebilirlik: ApplicationUser sınıfına(Identity Server tarafında) eklenen özel alanların(TC, şehir vb.)
    // frontend tarafında da karşılık bulması için UserDetailViewModel oluşturuldu.
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; } 
    }
}
