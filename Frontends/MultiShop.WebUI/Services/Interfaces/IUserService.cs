using Microsoft.EntityFrameworkCore.Metadata;
using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Services.Interfaces
{
    //Kullanıcı bilgilerini getirecek metodun imzası.
    //Service-Oriented Mimari: Kullanıcı verilerini çekme işi doğrudan Controller içinde yapılmadı;
    //bunun yerine bir IUserService tanımlanarak iş mantığı ayrıştırıldı(Separation of Concerns).
    public interface IUserService
    {
        Task<UserDetailViewModel> GetUserInfo();
    }
}
