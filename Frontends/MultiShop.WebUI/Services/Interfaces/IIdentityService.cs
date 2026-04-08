using MultiShop.DtoLayer.IdentityDtos.LogInDtos;

namespace MultiShop.WebUI.Services.Interfaces
{
    public interface IIdentityService
    {
        // burada kullanıcı girişi için gerekli metodu tanımlayacağız
         Task<bool> SignIn(SignInDto signInDto);
    }
}
