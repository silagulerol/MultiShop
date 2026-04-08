namespace MultiShop.WebUI.Settings
{
    // ClientSettings sınıfı, JSON formatındaki ClientSettings nesnesini temsil eder.
    // JSON formatında ClientSettings nesnesi, bir veya daha fazla Client içerebilir.
    public class ClientSettings
    {
        public Client MultiShopVisitorClient { get; set; }
        public Client MultiShopManagerClient { get; set; }
        public Client MultiShopAdminClient { get; set; }
    }

    /*
     * JSON formatı açısından baktığında ClientSettings süslü parantezlerle { } çevrelenmiş bir nesnedir. 
     * İçinde anahtar-değer (key-value) çiftleri barındırır.
     * 
     * Sen bu yapıyı C# koduna aktarırken iki yoldan birini seçebilirsin:
        A) Sınıf (Class) Yaklaşımı (Bizim yaptığımız): settings.MultiShopVisitorClient.ClientId diyerek noktayla (strongly typed) ilerleyebiliyorsun.
        B) Dictionary Yaklaşımı: Eğer kaç tane Client olacağını önceden kestiremiyorsan veya isimleri dinamikse, bunu şu şekilde de karşılayabilirdin:
                public Dictionary<string, Client> ClientSettings { get; set; }
                Veriye _settings["MultiShopVisitorClient"] şeklinde (string key ile) ulaşılır.
     */
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
