namespace MultiShop.IdentityServer.Tools
{
    public class JwtTokenDefaults
    {
        // tokenımızı kim yayınlıyor
        public const string ValidAudience = "http://localhost";
        // kim dinler
        public const string ValidIssuer = "http://localhost";
        public const string Key = "Multishop..0102030405Asp.NetCore6.0.28*+-";
        public const int Expire = 60; // tokenın geçerlilik süresi (dakika)

    }
}
