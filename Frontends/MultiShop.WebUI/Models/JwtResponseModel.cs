namespace MultiShop.WebUI.Models
{
    //Manuel olarak token oluşturmak için oluşturuldu bu model
    public class JwtResponseModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
